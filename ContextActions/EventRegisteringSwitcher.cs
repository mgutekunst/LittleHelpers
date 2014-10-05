using System;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace LittleHelpers.ContextActions
{
    [ContextAction(Description = "Switches the (de)registration of an EventHandler (+= <=> -=)",
      Group = "C#",
      Name = "EventRegisteringSwitcher ")]
    public sealed class EventRegisteringSwitcher : IContextAction
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private IBulbAction[] _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
        /// be injected in this constructor.
        /// </summary>
        public EventRegisteringSwitcher(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }


        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return Items.ToContextAction();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            var assignmentExpression = _provider.GetSelectedElement<IAssignmentExpression>(true, true);
            if (assignmentExpression != null)
            {
                return assignmentExpression.AssignmentType == AssignmentType.MINUSEQ || assignmentExpression.AssignmentType == AssignmentType.PLUSEQ;
            }
            return false;
        }

        private IBulbAction[] Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new IBulbAction[]
                   {
                     new SwitchEventHandlerAssignmantBulbItem(_provider)
                   };
                }
                return _items;
            }
        }
    }

    public class SwitchEventHandlerAssignmantBulbItem : BulbActionBase
    {
        private readonly ICSharpContextActionDataProvider _provider;

        public SwitchEventHandlerAssignmantBulbItem(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }

        public override string Text
        {
            get
            {
                // text returned here will be displayed on the context action
                return "(De)Register EventHandler";
            }
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var assignmentExpression = _provider.GetSelectedElement<IAssignmentExpression>(true, true);
            if (assignmentExpression != null)
            {
                var fact = CSharpElementFactory.GetInstance(_provider.PsiModule);
                var source = assignmentExpression.Source;
                var dest = assignmentExpression.Dest;


                ICSharpExpression statement = null;
                if (assignmentExpression.AssignmentType == AssignmentType.MINUSEQ)
                {
                    statement = fact.CreateExpression("$0 += $1", dest, source);
                }
                else if (assignmentExpression.AssignmentType == AssignmentType.PLUSEQ)
                {
                    statement = fact.CreateExpression("$0 -= $1", dest, source);
                }

                if(statement != null)
                    assignmentExpression.ReplaceBy(statement);
            }

            return null;

        }

    }
}
