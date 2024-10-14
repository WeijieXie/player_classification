import math
import os.path
import datetime
import numpy as np
import pandas as pd


class DataProcessor():
    def __init__(self, file_name, index, head, tail, save_dir, seq_length):
        self.df = pd.read_csv(file_name, low_memory=False, header=0)
        self.temp_df = self.df
        self.temp_df_ndarry = self.temp_df.values
        self.index = index
        self.head = head
        self.tail = tail
        self.save_dir = save_dir
        self.index_no = len(index)
        self.seq_length = seq_length


    # 选择要用的列
    def col_selector(self):
        self.temp_df = self.temp_df.get(self.index)
        return self.temp_df

    # 掐头去尾
    def row_cutter(self):
        self.temp_df = self.temp_df.loc[self.head:self.tail, :]
        return self.temp_df

    # 转换为极坐标，返回（n,2）
    def vector_to_pole(self, index1, index2):
        col1 = self.temp_df.loc[:, index1]
        col2 = self.temp_df.loc[:, index2]
        distance = np.sqrt(np.power(col1, 2) + np.power(col2, 2))
        angle = np.arctan2(col2, col1)
        return np.c_[angle, distance]

    # 全部转换为极坐标
    def vectors_to_pole(self):
        player = self.vector_to_pole('player_x', 'player_y')
        enemy1 = self.vector_to_pole('enemy1_to_player_x', 'enemy1_to_player_y')
        enemy2 = self.vector_to_pole('enemy2_to_player_x', 'enemy2_to_player_y')
        goal_player = self.vector_to_pole('goal_to_player_x', 'goal_to_player_y')
        goal_des = self.vector_to_pole('goal_to_des_x', 'goal_to_des_y')
        player_des = self.vector_to_pole('player_to_des_x', 'player_to_des_y')
        poles_np = np.concatenate((player, enemy1, enemy2, goal_player, goal_des, player_des), axis=1)
        poles_pd = pd.DataFrame(poles_np, columns=['player_x', 'player_y',
                                                   'enemy1_to_player_x', 'enemy1_to_player_y',
                                                   'enemy2_to_player_x', 'enemy2_to_player_y',
                                                   'goal_to_player_x', 'goal_to_player_y',
                                                   'goal_to_des_x', 'goal_to_des_y',
                                                   'player_to_des_x', 'player_to_des_y'])
        self.temp_df = pd.concat(
            [self.temp_df.loc[:, 'lable0':'Timestamp'], poles_pd, self.temp_df.loc[:, 'input_x':'input_all']], axis=1)
        return self.temp_df

    # 输入前移
    def input_forward(self, forward_steps):
        row_number = self.temp_df.shape[0]
        # vec_part = self.temp_df.iloc[:, 5:]
        # ['lable0','lable1', 'No', 'Timestamp', 'player_x', 'player_y', 'enemy1_to_player_x', 'enemy1_to_player_y', 'enemy2_to_player_x', 'enemy2_to_player_y', 'goal_to_player_x', 'goal_to_player_y', 'player_to_des_x', 'player_to_des_y']
        # input_part = self.temp_df.iloc[:, :15]
        # ['input_x', 'input_y', 'input_z', 'input_w', 'input_all']
        # right = input_part.loc[forward_steps:row_number, :]
        # left = vec_part.loc[1:row_number - forward_steps, :]
        left = self.temp_df.loc[:, 'lable0':'player_to_des_y']
        right = self.temp_df.loc[:, 'input_x':'input_all']

        left = left.loc[0:row_number - forward_steps - 1, :]
        right = right.loc[forward_steps:row_number - 1, :]

        # 重置索引值
        left = left.reset_index(drop=True)
        right = right.reset_index(drop=True)

        self.temp_df = pd.concat([left, right], axis=1)
        return self.temp_df

    # 差值检测
    def deviation_calculator(self):
        minuend = self.temp_df.loc[:, 'Timestamp':'player_to_des_y']
        subtract = minuend
        subtract = pd.DataFrame(np.insert(subtract.values, 0, values=[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], axis=0),
                                columns=['Timestamp', 'player_x', 'player_y', 'enemy1_to_player_x',
                                         'enemy1_to_player_y', 'enemy2_to_player_x', 'enemy2_to_player_y',
                                         'goal_to_player_x', 'goal_to_player_y', 'goal_to_des_x', 'goal_to_des_y',
                                         'player_to_des_x', 'player_to_des_y'])
        result = minuend.sub(subtract)
        self.temp_df = pd.concat(
            [self.temp_df.loc[:, 'lable0':'No'], result, self.temp_df.loc[:, 'input_x':'input_all']], axis=1)
        return self.temp_df

    #  def input_change_detector(self):

    # 保存
    def save(self):
        path = os.path.join(self.save_dir, datetime.datetime.now().strftime("%Y%m%d%H%M%S%f") + ".csv")
        self.temp_df.to_csv(path, index=False)
        print('......saved......')

    # 顺序取数
    def next_window(self, i):
        window = self.temp_df_ndarry[i:i + self.seq_length, :]
        # print('win')
        # print(window)
        # print(window.shape)

        x = window[:, 0:]
        y = window[0, 0]

        # print('x')
        # print(x)
        # print(x.shape)
        #
        # print('y')
        # print(y)
        # print(y.shape)

        return x, y

    # LSTM的输入格式
    def lstm_input_convertor(self):
        data_x = []
        data_y = []
        self.temp_df_ndarry = self.temp_df.values
        # print(self.temp_df_ndarry.shape)
        length=self.temp_df.shape[0]
        for i in range(0, length - self.seq_length, 100):
            x_win, y_win = self.next_window(i)
            # print(i)
            data_x.append(x_win)
            data_y.append(y_win)

        return np.array(data_x), np.array(data_y)


if __name__ == '__main__':
    a = DataProcessor('raw_training_data/record1.csv',
                      index=['Timestamp',
                             'player_x', 'player_y',
                             'enemy1_to_player_x', 'enemy1_to_player_y',
                             'enemy2_to_player_x', 'enemy2_to_player_y',
                             'goal_to_player_x', 'goal_to_player_y',
                             'goal_to_des_x', 'goal_to_des_y',
                             'player_to_des_x', 'player_to_des_y'],
                      head=10000,
                      tail=310000,
                      save_dir='training_data',
                      seq_length=2000)
    a.vectors_to_pole()
    x, y = a.lstm_input_convertor()
    print(x.shape)
    print(y.shape)
    # print(a.selected_df)
    # b = a.selected_df.loc[:, 'enemy1_to_player_x']
    # c = a.selected_df.loc[:, 'enemy1_to_player_y']
    # d = np.sqrt(np.power(b, 2) + np.power(c, 2))
    # e = np.arctan2(c, b)
    # f = pd.DataFrame(np.c_[d, e])

    # a.col_selector(['enemy1_to_player_x', 'enemy1_to_player_y'])
    # m = a.vector_to_pole('enemy1_to_player_x', 'enemy1_to_player_y')
    # print(m)
    # a.col_selector('enemy1_to_player_x')
    # print(m)
    # n = pd.DataFrame(m)
    # print(n)
