using System;
using System.Collections.Generic;
using System.Text;

namespace stone.app
{
    public interface TokenReader
    {
        /// <summary>
        /// 返回token流中的下一个token， 并从流中取出。如果流已为空，则返回null
        /// </summary>
        /// <returns></returns>
        public Token Read();

        /// <summary>
        /// 返回token流中的下一个token， 不从流中取出，如果已经为空，则返回null
        /// </summary>
        /// <returns></returns>
        public Token Peek();

        /// <summary>
        /// Token流回退一步，恢复原来的Token
        /// </summary>
        public void UnRead();

        /// <summary>
        /// 获取token流当前的读取位置
        /// </summary>
        /// <returns></returns>
        public int GetPosition();


        /// <summary>
        /// 设置token流当前的读取位置
        /// </summary>
        public void SetPosition(int position);

    }
}
