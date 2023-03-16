using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class PostEffectsBase : MonoBehaviour {

    // Called when start
    protected void CheckResources() {
        bool isSupported = CheckSupport();//判断是否支持
		
        if (isSupported == false) {
            NotSupported();
        }
    }

    // Called in CheckResources to check support on this platform
    protected bool CheckSupport() 
    {
        return true;
    }

    // Called when the platform doesn't support this effect
    protected void NotSupported() {
        enabled = false;
    }
	
    protected void Start() {
        CheckResources();//开始调用函数
    }

    // Called when need to create the material used by this effect
    protected Material CheckShaderAndCreateMaterial(Shader  shader, Material material) {//创建一个带有对应shader的材质。
        if (shader == null) {
            return null;
        }
		
        if (/*shader.isSupported &&*/ material && material.shader == shader)
            return material;
		
        /*if (!shader.isSupported) {
            return null;}*/
		
        else {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;//该对象不保存到场景。加载新场景时，也不会销毁它
            if (material)
                return material;
            else 
                return null;
        }
    }
}
