using System;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Psi.IL.Parsing;
using JetBrains.ReSharper.Psi.Parsing;

namespace LittleHelpers.RS9.ContextActions
{
    [ContextAction(Description = "Switches true to false and vice versa",
    Group = "C#",
    Name = "TrueFalseSwitcher")]
    public sealed class TrueFalseSwitcher : BulbActionBase,IContextAction
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
            return this.ToContextActionIntentions();
        }

        public bool IsAvailable(IUserDataHolder cache)
        {
            _lit = _provider.GetSelectedElement<ICSharpLiteralExpression>(true, true);
            if (_lit != null)
            {
                TokenNodeType token = _lit.Literal.GetTokenType();
            
                if (token.TokenRepresentation == "true"|| token.TokenRepresentation == "false")
                    return true;
            }
            return false;
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
            if (lit != null)
            {
                var factory = CSharpElementFactory.GetInstance(_provider.PsiModule);

                ICSharpExpression newLit = null;
                if (lit.Literal.GetTokenType() == ILTokenType.TRUE_KEYWORD)
                {
                    newLit = factory.CreateExpressionAsIs("false");
                }
                else if (lit.Literal.GetTokenType() == ILTokenType.FALSE_KEYWORD)
                {
                    newLit = factory.CreateExpressionAsIs("true");
                }
                if (newLit != null)
                    lit.ReplaceBy(newLit);
            }
            return null;
        }

    }
}