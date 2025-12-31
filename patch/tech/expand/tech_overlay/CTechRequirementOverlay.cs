using SCPlus.patch.game_event;
using System.Collections.Generic;
using static SCPlus.patch.variable.VariableBuilder;

namespace SCPlus.patch.tech.expand.tech_overlay
{
    internal class CTechRequirementOverlay : CTechSelectionOverlay
    {
        public override void ClickedApply()
        {
            base.ClickedApply();

            if (!Valid())
            {
                Plugin.LogError($"{nameof(ClickedApply)}: Missing screen");
                return;
            }

            Technology tech = screen.selectedHex?.tech;
            if (tech == null)
            {
                return;
            }

            EventNode techAnd = SyntaxTree.Children[0];
            EventNode techOr = SyntaxTree.Children[1];

            SaveData(techAnd, techOr, tech);
        }

        public override void Enter()
        {
            base.Enter();

            if (!Valid())
            {
                Plugin.LogError($"{nameof(Enter)}: Missing screen");
                return;
            }

            Technology tech = screen.selectedHex?.tech;
            if (tech == null)
            {
                return;
            }

            CollectData(tech);

            // Ensure collision on tech tree does not occur while setting up requirements
            screen.techTree.enabled = false;
        }

        public override void Exit()
        {
            base.Exit();

            if (!Valid())
            {
                Plugin.LogError($"{nameof(Exit)}: Missing screen");
                return;
            }

            screen.techTree.enabled = true;
        }

        internal virtual void SaveData(EventNode techAnd, EventNode techOr, Technology tech)
        {
            SaveTech(techAnd, techOr, out tech.requiredTechAND, out tech.techRequirementAnd, out tech.requiredTechOR, out tech.techRequirementOr);
        }

        internal virtual void CollectData(Technology tech)
        {
            SetData(CollectConditions(tech.requiredTechAND, tech.requiredTechOR));
        }

        internal void SaveTech(EventNode techAnd, EventNode techOr, out List<string> techAndList, out string techAndString, out List<string> techOrList, out string techOrString)
        {
            techAndList = [];
            techOrList = [];

            foreach (EventNode node in techAnd.Children)
            {
                techAndList.Add(node.Data);
            }

            foreach (EventNode node in techOr.Children)
            {
                techOrList.Add(node.Data);
            }

            techAndString = screen.RequiredTechArrayToString(techAndList);
            techOrString = screen.RequiredTechArrayToString(techOrList);
        }

        internal ParameterCondition CollectConditions(List<string> techAnd, List<string> techOr)
        {
            List<ParameterCondition> conditionList = [];

            if (techAnd != null)
            {
                foreach (string id in techAnd)
                {
                    conditionList.Add(new()
                    {
                        techTrigger = new()
                        {
                            triggered = true,
                            techID = id
                        }
                    });
                }
            }

            if (techOr != null)
            {
                foreach (string id in techOr)
                {
                    conditionList.Add(new()
                    {
                        techTrigger = new()
                        {
                            triggered = false,
                            techID = id
                        }
                    });
                }
            }

            return new()
            {
                parameterConditions = [.. conditionList]
            };
        }

        protected bool Valid()
        {
            return screen != null;
        }

        internal TechManagementScreen screen = null;
    }
}
