# 填补空缺算法

## 需求：

在每次执行元素消除后，需要对消除后矩阵进行补全

## 算法

基础数据结构：列表

思路：以列为单位，计算各列缺少的元素个数，并按列进行恢复

### 步骤

1. 统计每一列缺少的元素个数
   
   ![1.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Filling%20Vacancies%20Figs/1.jpg)

2. 将每一列还存在元素填充到列头，留出列尾位置
   
   ![2.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Filling%20Vacancies%20Figs/2.jpg)

3. 根据每列缺少的元素个数，随机生成不同类型的元素，填补到列尾空缺位置
   
   ![3.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Filling%20Vacancies%20Figs/3.jpg)

### 不足

1. 步骤2、步骤3需要控制移动动画播放，而步骤2中，位置不需要改变的元素不需要播放动画，所以这里需要重新统计一次播放动画的数量，消耗了时间
