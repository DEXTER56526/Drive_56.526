using System;
using UnityEngine;
using UnityEngine.UI;

// Simple garage manager that lists available vehicle prefabs and lets the player choose one.
// The UI is expected to be a vertical layout with a button prefab that calls SelectVehicle(index).
public class GarageManager : MonoBehaviour
{
    [Header("Vehicles")]
    public GameObject[] vehiclePrefabs;

    [Header("UI")]
    public Transform listContent; // UI content transform where buttons will be created
    public Button buttonTemplate; // assign a Button prefab with a Text child

    const string PrefKeySelected = "Garage_SelectedVehicle";

    void Start()
    {
        PopulateUI();
    }

    void PopulateUI()
    {
        if (listContent == null || buttonTemplate == null) return;

        // Clear existing
        foreach(Transform t in listContent) Destroy(t.gameObject);

        for (int i = 0; i < vehiclePrefabs.Length; i++)
        {
            GameObject go = Instantiate(buttonTemplate.gameObject, listContent);
            Button b = go.GetComponent<Button>();
            int idx = i;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => SelectVehicle(idx));

            Text txt = go.GetComponentInChildren<Text>();
            if (txt != null) txt.text = vehiclePrefabs[i] != null ? vehiclePrefabs[i].name : "Vehicle " + idx;
            go.SetActive(true);
        }

        // Highlight current selection (optional)
    }

    public void SelectVehicle(int index)
    {
        if (index < 0 || index >= vehiclePrefabs.Length) return;
        PlayerPrefs.SetInt(PrefKeySelected, index);
        PlayerPrefs.Save();
        Debug.Log("Selected vehicle: " + vehiclePrefabs[index].name);
    }

    public GameObject GetSelectedVehiclePrefab()
    {
        int idx = PlayerPrefs.GetInt(PrefKeySelected, 0);
        if (vehiclePrefabs == null || vehiclePrefabs.Length == 0) return null;
        return vehiclePrefabs[Mathf.Clamp(idx, 0, vehiclePrefabs.Length-1)];
    }
}
