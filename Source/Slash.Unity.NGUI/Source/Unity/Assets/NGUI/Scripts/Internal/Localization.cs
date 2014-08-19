//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

//#define SHOW_REPORT

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Localization manager is able to parse localization information from text assets.
/// Using it is simple: text = Localization.Get(key), or just add a UILocalize script to your labels.
/// You can switch the language by using Localization.language = "French", for example.
/// This will attempt to load the file called "French.txt" in the Resources folder,
/// or a column "French" from the Localization.csv file in the Resources folder.
/// If going down the TXT language file route, it's expected that the file is full of key = value pairs, like so:
/// 
/// LABEL1 = Hello
/// LABEL2 = Music
/// Info = Localization Example
/// 
/// In the case of the CSV file, the first column should be the "KEY". Other columns
/// should be your localized text values, such as "French" for the first row:
/// 
/// KEY,English,French
/// LABEL1,Hello,Bonjour
/// LABEL2,Music,Musique
/// Info,"Localization Example","Par exemple la localisation"
/// </summary>

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	static Localization mInstance;

	/// <summary>
	/// List of loaded languages. Available if a single Localization.csv file was used.
	/// </summary>

	static public string[] knownLanguages;

	/// <summary>
	/// Localization dictionary. Dictionary key is the localization key. Dictionary value is the list of localized values (columns in the CSV file).
	/// Be very careful editing this via code, and be sure to set the "KEY" to the list of languages, and set localizationHasBeenSet to 'true'.
	/// </summary>

	static public Dictionary<string, string[]> dictionary
	{
		get
		{
			if (!localizationHasBeenSet) language = PlayerPrefs.GetString("Language", "English");
			return mDictionary;
		}
	}

	/// <summary>
	/// Whether the localization dictionary has been set already.
	/// </summary>

	static public bool localizationHasBeenSet = false;

	/// <summary>
	/// Whether there is an instance of the localization class present.
	/// </summary>

	static public bool isActive { get { return mInstance != null; } }

	/// <summary>
	/// The instance of the localization class. Will create it if one isn't already around.
	/// </summary>

	static public Localization instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = Object.FindObjectOfType(typeof(Localization)) as Localization;

				if (mInstance == null)
				{
					GameObject go = new GameObject("_Localization");
					DontDestroyOnLoad(go);
					mInstance = go.AddComponent<Localization>();
				}
			}
			return mInstance;
		}
	}

	// Deprecated functionality. Set the starting language yourself via Localization.language = "Starting Language".
	[HideInInspector] public string startingLanguage = "English";

	// Deprecated functionality. No need to set languages anymore. Just place them in the Resources folder, or use a global Localization CSV file.
	[HideInInspector] public TextAsset[] languages;

	// Key = Value dictionary (single language)
	static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();
	static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
	static int mLanguageIndex = -1;
	static string mLanguage;

#if SHOW_REPORT
	BetterList<string> mUsed = new BetterList<string>();
#endif

	/// <summary>
	/// Determine the starting language.
	/// </summary>

	void Awake ()
	{
		if (mInstance == null)
		{
			mInstance = this;
			DontDestroyOnLoad(gameObject);

			// Legacy functionality
			if (mOldDictionary.Count == 0 && mDictionary.Count == 0)
				language = PlayerPrefs.GetString("Language", startingLanguage);

			// Legacy functionality
			if (string.IsNullOrEmpty(mLanguage) && (languages != null && languages.Length > 0))
				language = languages[0].name;
		}
		else Destroy(gameObject);
	}

	/// <summary>
	/// Oddly enough... sometimes if there is no OnEnable function in Localization, it can get the Awake call after UILocalize's OnEnable.
	/// </summary>

	void OnEnable () { if (mInstance == null) mInstance = this; }

#if SHOW_REPORT
	/// <summary>
	/// It's often useful to be able to tell which keys are used in localization, and which are not.
	/// For this to work properly it's advised to play through the entire game and view all localized content before hitting the Stop button.
	/// </summary>

	void OnDisable ()
	{
		string final = "";
		BetterList<string> full = new BetterList<string>();

		// Create a list of all the known keys
		foreach (KeyValuePair<string, string> pair in mDictionary) full.Add(pair.Key);

		// Sort the full list
		full.Sort(delegate(string s1, string s2) { return s1.CompareTo(s2); });

		// Create the final string with the localization keys
		for (int i = 0; i < full.size; ++i)
		{
			string key = full[i];
			string val = mDictionary[key].Replace("\n", "\\n");
			if (mUsed.Contains(key)) final += key + " = " + val + "\n";
			else final += "//" + key + " = " + val + "\n";
		}
		
		// Show the final report in a format that makes it easy to copy/paste into the original localization file
		if (!string.IsNullOrEmpty(final))
			Debug.Log("// Localization Report\n\n" + final);

		mLocalizationLoaded = false;
		mLanguageIndex = -1;
		mLocalization.Clear();
		mDictionary.Clear();
	}
#else
	/// <summary>
	/// Clear the dictionaries.
	/// </summary>

	void OnDisable ()
	{
		localizationHasBeenSet = false;
		mLanguageIndex = -1;
		mDictionary.Clear();
		mOldDictionary.Clear();
	}
#endif

	/// <summary>
	/// Remove the instance reference.
	/// </summary>

	void OnDestroy () { if (mInstance == this) mInstance = null; }

	/// <summary>
	/// Name of the currently active language.
	/// </summary>

	[System.Obsolete("Use Localization.language instead")]
	public string currentLanguage { get { return language; } set { language = value; } }

	/// <summary>
	/// Name of the currently active language.
	/// </summary>

	static public string language
	{
		get
		{
			return mLanguage;
		}
		set
		{
			if (mLanguage != value)
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (mDictionary.Count == 0)
					{
						// Try to load the Localization CSV
						TextAsset txt = localizationHasBeenSet ? null : Resources.Load("Localization", typeof(TextAsset)) as TextAsset;
						localizationHasBeenSet = true;

						if (txt == null || !LoadCSV(txt))
						{
							// Not a referenced asset -- try to load it dynamically
							txt = Resources.Load(value, typeof(TextAsset)) as TextAsset;

							if (txt != null)
							{
								Load(txt);
								return;
							}
						}
					}

					// Try to load the language from the CSV list
					if (mDictionary.Count != 0 && SelectLanguage(value)) return;

					// Legacy functionality where languages were specified on the Localization class
					if (mInstance != null && mInstance.languages != null)
					{
						for (int i = 0, imax = mInstance.languages.Length; i < imax; ++i)
						{
							TextAsset asset = mInstance.languages[i];

							if (asset != null && asset.name == value)
							{
								Load(asset);
								return;
							}
						}
					}
				}

				// Either the language is null, or it wasn't found
				mOldDictionary.Clear();
				PlayerPrefs.DeleteKey("Language");
			}
		}
	}

	/// <summary>
	/// Load the specified asset and activate the localization.
	/// </summary>

	static public void Load (TextAsset asset)
	{
		ByteReader reader = new ByteReader(asset);
		Set(asset.name, reader.ReadDictionary());
	}

	/// <summary>
	/// Load the specified CSV file.
	/// </summary>

	static public bool LoadCSV (TextAsset asset)
	{
#if SHOW_REPORT
		mUsed.Clear();
#endif
		ByteReader reader = new ByteReader(asset);

		// The first line should contain "KEY", followed by languages.
		BetterList<string> temp = reader.ReadCSV();

		// There must be at least two columns in a valid CSV file
		if (temp.size < 2) return false;

		// The first entry must be 'KEY', capitalized
		temp[0] = "KEY";

#if !UNITY_3_5
		// Ensure that the first value is what we expect
		if (!string.Equals(temp[0], "KEY"))
		{
			Debug.LogError("Invalid localization CSV file. The first value is expected to be 'KEY', followed by language columns.\n" +
				"Instead found '" + temp[0] + "'", asset);
			return false;
		}
		else
#endif
		{
			knownLanguages = new string[temp.size - 1];
			for (int i = 0; i < knownLanguages.Length; ++i)
				knownLanguages[i] = temp[i + 1];
		}

		mDictionary.Clear();

		// Read the entire CSV file into memory
		while (temp != null)
		{
			AddCSV(temp);
			temp = reader.ReadCSV();
		}
		return true;
	}

	/// <summary>
	/// Select the specified language from the previously loaded CSV file.
	/// </summary>

	static bool SelectLanguage (string language)
	{
		mLanguageIndex = -1;

		if (mDictionary.Count == 0) return false;

		string[] keys;

		if (mDictionary.TryGetValue("KEY", out keys))
		{
			for (int i = 0; i < keys.Length; ++i)
			{
				if (keys[i] == language)
				{
					mOldDictionary.Clear();
					mLanguageIndex = i;
					mLanguage = language;
					PlayerPrefs.SetString("Language", mLanguage);
					UIRoot.Broadcast("OnLocalize");
					return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Helper function that adds a single line from a CSV file to the localization list.
	/// </summary>

	static void AddCSV (BetterList<string> values)
	{
		if (values.size < 2) return;
		string[] temp = new string[values.size - 1];
		for (int i = 1; i < values.size; ++i) temp[i - 1] = values[i];
		mDictionary.Add(values[0], temp);
	}

	/// <summary>
	/// Load the specified asset and activate the localization.
	/// </summary>

	static public void Set (string languageName, Dictionary<string, string> dictionary)
	{
#if SHOW_REPORT
		mUsed.Clear();
#endif
		mLanguage = languageName;
		PlayerPrefs.SetString("Language", mLanguage);
		mOldDictionary = dictionary;
		localizationHasBeenSet = false;
		mLanguageIndex = -1;
		knownLanguages = new string[] { languageName };
		UIRoot.Broadcast("OnLocalize");
	}

	/// <summary>
	/// Localize the specified value.
	/// </summary>

	static public string Get (string key)
	{
		// Ensure we have a language to work with
		if (!localizationHasBeenSet) language = PlayerPrefs.GetString("Language", "English");

#if SHOW_REPORT
		if (!mUsed.Contains(key)) mUsed.Add(key);
#endif
		string val;
		string[] vals;
#if UNITY_IPHONE || UNITY_ANDROID
		string mobKey = key + " Mobile";

		if (mLanguageIndex != -1 && mDictionary.TryGetValue(mobKey, out vals))
		{
			if (mLanguageIndex < vals.Length)
				return vals[mLanguageIndex];
		}
		else if (mOldDictionary.TryGetValue(mobKey, out val)) return val;
#endif
		if (mLanguageIndex != -1 && mDictionary.TryGetValue(key, out vals))
		{
			if (mLanguageIndex < vals.Length)
				return vals[mLanguageIndex];
		}
		else if (mOldDictionary.TryGetValue(key, out val)) return val;

#if UNITY_EDITOR
		Debug.LogWarning("Localization key not found: '" + key + "'");
#endif
		return key;
	}

	/// <summary>
	/// Localize the specified value.
	/// </summary>

	[System.Obsolete("Use Localization.Get instead")]
	static public string Localize (string key) { return Get(key); }

	/// <summary>
	/// Returns whether the specified key is present in the localization dictionary.
	/// </summary>

	static public bool Exists (string key)
	{
		if (mLanguageIndex != -1) return mDictionary.ContainsKey(key);
		return mOldDictionary.ContainsKey(key);
	}
}
