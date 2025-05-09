using SiggaBlog.Commons.Permissions;
using System.Windows.Input;

namespace MainApp.Input
{
    public class ObservableCommand : ICommand, IPermissionsObject
    {
        readonly bool _validatePermissionOnExecute;

        private readonly Action<object, PermissionArgs>? _handler;
        private readonly Func<object, PermissionArgs, Task>? _asyncHandler;
        public event EventHandler? CanExecuteChanged;

        private PermissionsStatusResult _permissionsStatus;

        public PermissionsStatusResult PermissionsStatus
        {
            get { return _permissionsStatus; }
            set { _permissionsStatus = value; }
        }

        public ObservableCommand(Action<object, PermissionArgs> handler)
        {
            this._handler = handler;
        }

        public ObservableCommand(Func<object, PermissionArgs, Task> handler)
        {
            this._asyncHandler = handler;
        }

        public ObservableCommand(Action<object, PermissionArgs> handler, bool validatePermissionsOnExecute) : this(handler)
        {
            this._validatePermissionOnExecute = validatePermissionsOnExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            this._asyncHandler?.Invoke(parameter, new PermissionArgs(this.PermissionsStatus));
            this._handler?.Invoke(parameter, new PermissionArgs(this.PermissionsStatus));
        }
    }
}
