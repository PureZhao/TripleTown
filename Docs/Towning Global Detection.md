# 全局检测算法

## 需求：

在每次执行消除后，已有元素下移和新生成的元素被填补上来后，可能会产生新的能够消除的位置，需要继续消除，直到没有符合可以消除的位置产生。

## 算法

基础数据结构：栈

思路：对元素矩阵的每一行每一列，进行扫描，时间复杂度O(n)

### 步骤

1. 先将第一个位置压栈
   
   ![1.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Towning%20Global%20Detection%20Figs/1.jpg)

2. 从第二个位置元素开始遍历整个序列，与栈顶元素的类型进行对比
   
   - 如果两者类型相同，将元素压栈，并继续判断下一个元素
     
     ![4.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Towning%20Global%20Detection%20Figs/4.jpg)
   
   - 如果不同，判断目前栈中元素的数量
     
     - 如果数量大于等于3，则代表其中的元素可以消除，记录栈中元素，并清空栈，再将该位置元素压入栈
       
       ![5.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Towning%20Global%20Detection%20Figs/5.jpg)
     
     - 如果数量小于3，直接清空栈，将该位置元素压入栈
       
       ![3.jpg](https://github.com/PureZhao/TripleTown/raw/main/Docs/Towning%20Global%20Detection%20Figs/3.jpg)

3. 当遍历结束，再对栈做一次步骤2中的数量判断

### 不足

1. 需要对全盘元素做两次遍历

2. 行列检测完毕后，需要对元素进行去重操作
