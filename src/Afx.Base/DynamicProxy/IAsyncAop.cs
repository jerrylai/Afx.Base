using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Afx.DynamicProxy
{
    /// <summary>
    /// IAsyncAop 接口
    /// </summary>
    public interface IAsyncAop
    {
        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="context">Aop上下文</param>
        Task OnExecuting(AopContext context);

        /// <summary>
        /// 方法执行后
        /// </summary>
        /// <param name="context">Aop上下文</param>
        /// <param name="returnValue">返回对象</param>
        Task OnResult(AopContext context, object returnValue);

        /// <summary>
        /// 方法异常
        /// </summary>
        /// <param name="context">Aop上下文</param>
        /// <param name="ex">异常信息</param>
        Task OnException(AopContext context, Exception ex);
    }
}
