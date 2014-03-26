using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace LittleHelpers
{
    [ActionHandler("LittleHelpers.About")]
    public class AboutAction : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show(
              "Little Helpers\nManuel Gutekunst\n\nSmall Helpers to ease the the life of a programmer",
              "About Little Helpers",
              MessageBoxButtons.OK,
              MessageBoxIcon.Information);
        }
    }
}