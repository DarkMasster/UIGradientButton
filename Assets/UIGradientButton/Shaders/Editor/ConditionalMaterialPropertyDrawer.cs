using UnityEditor;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    public abstract class ConditionalMaterialPropertyDrawer : MaterialPropertyDrawer
    {
        protected abstract bool ShowIfKeywordSet { get; }
        protected abstract bool IsFloat { get; }

        protected virtual bool IsFloatValid(float val1, float val2)
        {
            return false;
        }

        private string _keyword;

        public ConditionalMaterialPropertyDrawer(string keyword)
        {
            _keyword = keyword;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (IsVisible(prop, label, editor))
            {
                editor.DefaultShaderProperty(position, prop, label);
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (IsVisible(prop, label, editor))
            {
                return base.GetPropertyHeight(prop, label, editor);
            }
            
            return -2f; // invert default line height
        }

        private bool IsVisible(MaterialProperty prop, string label, MaterialEditor editor)
        {
            var isKeywordEnabled = false;
            var isValidFloat = false;
            var floatProperty = "";
            var floatValue = 0f;

            if (IsFloat)
            {
                var keywordChunks = _keyword.Split('#');

                if (keywordChunks.Length > 1 && float.TryParse(keywordChunks[1], out floatValue))
                {
                    floatProperty = keywordChunks[0];
                    isValidFloat = true;
                }
            }
            
            for (var i = 0; i < editor.targets.Length; i++)
            {
                var mat = editor.targets[i] as Material;
                if (mat != null)
                {
                    if (!IsFloat)
                    {
                        isKeywordEnabled |= mat.IsKeywordEnabled(_keyword);
                    }
                    else if (isValidFloat)
                    {
                        isKeywordEnabled |= IsFloatValid(floatValue, mat.GetFloat(floatProperty));
                    }
                }
            }

            return ShowIfKeywordSet == isKeywordEnabled;
        }
    }

    public class ShowIfEnabled : ConditionalMaterialPropertyDrawer
    {
        public ShowIfEnabled(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => false;
    }
    
    public class ShowIfDisabled : ConditionalMaterialPropertyDrawer
    {
        public ShowIfDisabled(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => false;
        protected override bool IsFloat => false;
    }
    
    public class HideIfEnabled : ConditionalMaterialPropertyDrawer
    {
        public HideIfEnabled(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => false;
        protected override bool IsFloat => false;
    }
    
    public class HideIfDisabled : ConditionalMaterialPropertyDrawer
    {
        public HideIfDisabled(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => false;
    }
    
    public class ShowIfGreater : ConditionalMaterialPropertyDrawer
    {
        public ShowIfGreater(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => true;
        protected override bool IsFloatValid(float val1, float val2)
        {
            return val1 < val2;
        }
    }
    
    public class ShowIfGreaterEqual : ConditionalMaterialPropertyDrawer
    {
        public ShowIfGreaterEqual(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => true;
        protected override bool IsFloatValid(float val1, float val2)
        {
            return val1 <= val2;
        }
    }
    
    public class ShowIfEqual : ConditionalMaterialPropertyDrawer
    {
        public ShowIfEqual(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => true;
        protected override bool IsFloatValid(float val1, float val2)
        {
            return Mathf.Approximately(val1, val2);
        }
    }
    
    public class ShowIfLess : ConditionalMaterialPropertyDrawer
    {
        public ShowIfLess(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => true;
        protected override bool IsFloatValid(float val1, float val2)
        {
            return val1 > val2;
        }
    }
    
    public class ShowIfLessEqual : ConditionalMaterialPropertyDrawer
    {
        public ShowIfLessEqual(string keyword) : base(keyword)
        {
        }

        protected override bool ShowIfKeywordSet => true;
        protected override bool IsFloat => true;
        protected override bool IsFloatValid(float val1, float val2)
        {
            return val1 >= val2;
        }
    }
}