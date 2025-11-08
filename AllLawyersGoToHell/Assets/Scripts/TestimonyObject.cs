using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TestimonyText")]

public class TestimonyObject : ScriptableObject
{
	[TextArea(15, 20)]
	[HideInInspector] public bool used = false;
	public string testimonyText;
	public int points;
}
