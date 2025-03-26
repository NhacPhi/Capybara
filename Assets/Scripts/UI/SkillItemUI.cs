using Core.Entities.Common;
using Core.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class SkillItemUI : ButtonBase
    {
        [field: SerializeField] public TextMeshProUGUI SkillName { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Description { get; private set; }
        public SkillData SkillData;
        public EntitySkill entities;
        private void Reset()
        {
            SkillName = transform.Find("Skill Name").GetComponent<TextMeshProUGUI>();
            Description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        }
    }
}