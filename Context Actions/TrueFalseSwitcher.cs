using System;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ResharperPlugins.Context_Actions
{

    [ContextAction(Description = "Switches true to false and vice versa",
      Group = "C#",
      Name = "TrueFalseSwitcher")]
    public sealed class TrueFalseSwitcher : IContextAction
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private ICSharpLiteralExpression _lit;
        private IBulbAction[] _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
        /// be injected in this constructor.
        /// </summary>
        public TrueFalseSwitcher(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }


        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return Items.ToContextAction();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            _lit = _provider.GetSelectedElement<ICSharpLiteralExpression>(true, true);
            if (_lit != null)
            {
                var token = _lit.Literal.GetTokenType();
                if (token == TokenType.TRUE_KEYWORD || token == TokenType.FALSE_KEYWORD)
                    return true;
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
                     new MyBulbItem(_provider)
                   };
                }
                return _items;
            }
        }
    }

    public class MyBulbItem : BulbActionBase
    {
        private readonly ICSharpContextActionDataProvider _provider;

        public MyBulbItem(ICSharpContextActionDataProvider provider)
        {
            _provider = provider;
        }

        public override string Text
        {
            get
            {
                // text returned here will be displayed on the context action
                return "Switch true/false";
            }
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            // put transacted steps here
            // use 'provider' field to get currently selected element
            var lit = _provider.GetSelectedElement<ICSharpLiteralExpression>(true, true);
            if(lit != null)
            {
                var factory = CSharpElementFactory.GetInstance(_provider.PsiModule);

                ICSharpExpression newLit = null;
                if(lit.Literal.GetTokenType() == TokenType.TRUE_KEYWORD)
                {
                    newLit = factory.CreateExpressionAsIs("false");
                }
                else if(lit.Literal.GetTokenType() == TokenType.FALSE_KEYWORD)
                {
                    newLit = factory.CreateExpressionAsIs("true");
                }
                if(newLit != null)
                    lit.ReplaceBy(newLit);
            }
            return null;
        }

    }
}
