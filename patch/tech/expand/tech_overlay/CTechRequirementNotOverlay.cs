namespace SCPlus.patch.tech.expand.tech_overlay
{
    internal class CTechRequirementNotOverlay : CTechRequirementOverlay
    {
        internal override void CollectData(Technology tech)
        {
            SetData(CollectConditions(tech.notTechAND, tech.notTechOR));
        }

        internal override void SaveData(EventNode techAnd, EventNode techOr, Technology tech)
        {
            SaveTech(techAnd, techOr, out tech.notTechAND, out tech.notRequirementAnd, out tech.notTechOR, out tech.notRequirementOr);
        }
    }
}
