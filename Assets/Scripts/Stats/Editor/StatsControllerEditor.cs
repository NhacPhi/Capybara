#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Stats.M_Attribute;
using Stats.stat;
using Tech.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Stats.Editor
{
	[CustomEditor(typeof(StatsController), true)]
	public class StatsControllerEditor : UnityEditor.Editor
	{
		private VisualElement _Root;
		private VisualElement _Head;
		private VisualElement _Body;

		private const string _errorMessage = "ID Not Found";
		private const string _allAttributes = "All Attribute: ";
		private const string _allStats = "All Stats: ";
		
		private string _search;
		private string text;
		private EntitiesStatsDataBase dataBase;
		private StatsDataHolder statsHolder;
		private SerializedProperty _idProperty;
		public override VisualElement CreateInspectorGUI()
	    {
			_Root = new VisualElement();
			_Head = new VisualElement();
			_Body = new VisualElement();


			_Root.Add(_Head);
			_Root.Add(_Body);

			try
			{
				text = File.ReadAllText("Assets/Data/EntitiesStats.json");
			}
			catch (Exception)
			{
				return null;
			}
			dataBase = Json.DeserializeObject<EntitiesStatsDataBase>(text);
			
			StringSearch search = ScriptableObject.CreateInstance<StringSearch>();
			search.Keys = new List<string>();
			search.Callback = (value) =>
			{
				_idProperty.stringValue = value;
				_idProperty.serializedObject.ApplyModifiedProperties();
			};
			
			foreach(var key in dataBase.EntitiesStats.Keys){
				search.Keys.Add(key);
			}

			var button = new Button(() => {
				SearchWindow.Open(new SearchWindowContext(GUIUtility
                    .GUIToScreenPoint(Event.current.mousePosition)), search);   
			});
			button.style.height = 20;
			button.text = "Search ID";
			var idField = new TextField();
			_idProperty = serializedObject.FindProperty("<EntityID>k__BackingField");
			idField.BindProperty(_idProperty);
			RefreshBody();

			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				_Head.style.flexDirection = FlexDirection.Column;
				_Head.Add(button);
				_Head.Add(idField);
			}
			
			idField.RegisterValueChangedCallback( (e) => {
				RefreshBody();
			});
			
			return _Root;
	    }

		private void RefreshBody()
		{
			serializedObject.Update();
			_Body.Clear();

			if(!dataBase.EntitiesStats.TryGetValue(_idProperty.stringValue, out statsHolder)){
				_Body.Add(new HelpBox(_errorMessage, HelpBoxMessageType.Error));
				return;
			}

			if(!target) return;
			bool playMode = EditorApplication.isPlayingOrWillChangePlaymode &&
							  !PrefabUtility.IsPartOfPrefabAsset(target);

			StatsController controller = target as StatsController;

			if (playMode)
			{
				_Body.Add(new M_AttributeView(controller));
				_Body.Add(new M_StatView(controller));
				_Body.Add(new M_StatusEffectView(controller));
				return;
			}

			InitBodyInEditor(statsHolder, controller);
		}

		private void InitBodyInEditor(StatsDataHolder statsHolder, StatsController controller)
		{
			var attributesView = new M_AttributeView();
			var statsView = new M_StatView();


			var labelAttribites = new Label(_allAttributes);
			labelAttribites.style.fontSize = 12;
			labelAttribites.style.unityFontStyleAndWeight = FontStyle.Bold;
			attributesView.Body.Add(labelAttribites);

			var labelStats = new Label(_allStats);
			labelStats.style.fontSize = 12;
			labelStats.style.unityFontStyleAndWeight = FontStyle.Bold;
			statsView.Body.Add(labelStats);

			foreach (AttributeType key in statsHolder.AttributeItems.Keys)
			{
				labelAttribites.text += key.ToString() + " | ";
			}

			foreach (StatType key in statsHolder.StatItems.Keys)
			{
				labelStats.text += key.ToString() + " | ";
			}

			_Body.Add(attributesView);
			_Body.Add(statsView);
		}
	}
}
#endif