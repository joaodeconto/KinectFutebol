using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(Acao))]
public class Opcoes : Editor {
		
	Acao 	acao;
	
	public void OnEnable()
    {
		acao = target as Acao;
    }
 	
 	public override void OnInspectorGUI () {
		GUILayoutOption[] layout = new GUILayoutOption[0];
		
		GUILayout.Label("Development by Visiorama: www.visiorama.com.br");
		acao.tipoDeAcao = (TipoDeAcao)EditorGUILayout.EnumPopup("Tipo de acao:", acao.tipoDeAcao, layout);
		
		EditorGUILayout.Separator();
		
		if(acao.tipoDeAcao == TipoDeAcao.MudarCena)
		{
			acao.tipoChamadaDeCena = (TipoChamadaDeCena)EditorGUILayout.EnumPopup("Cena tipo:", acao.tipoChamadaDeCena, layout);
			if (acao.tipoChamadaDeCena == TipoChamadaDeCena.String)
				acao.cenaStr = EditorGUILayout.TextField("Nome da cena:", acao.cenaStr, layout);
			else
				acao.cenaInt = EditorGUILayout.IntField("Num da cena:", acao.cenaInt, layout);
		}
		else if(acao.tipoDeAcao == TipoDeAcao.MudarCor)
		{
			acao.cor = EditorGUILayout.ColorField("Cor", acao.cor, layout);
		}
		else
		{
			acao.material = EditorGUILayout.ObjectField("Material", acao.material, typeof(Material), layout) as Material;
			acao.objetoOri = EditorGUILayout.ObjectField("Pers. Prin.", acao.objetoOri, typeof(GameObject), layout) as GameObject;
			acao.objetoRep = EditorGUILayout.ObjectField("Pers. Rep.", acao.objetoRep, typeof(GameObject), layout) as GameObject;
		}
	}
}