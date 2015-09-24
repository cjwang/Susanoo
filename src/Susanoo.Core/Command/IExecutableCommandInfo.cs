using System.Data.Common;

namespace Susanoo.Command
{
    /// <summary>
    /// Represents a ready to be executed CommandBuilder.
    /// </summary>
    public interface IExecutableCommandInfo
        : ICommandInfo
    {
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        DbParameter[] Parameters { get; }
    }
}