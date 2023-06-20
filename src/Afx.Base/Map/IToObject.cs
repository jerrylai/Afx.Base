using System;
using System.Collections.Generic;
using System.Text;

namespace Afx.Map
{
    /// <summary>
    /// map to model
    /// </summary>
    public interface IToObject
    {
        /// <summary>
        /// to model
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        object To(object fromObj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromObj"></param>
        /// <param name="setObj"></param>
        void Set(object fromObj, object setObj);
    }
}
