using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turno : MonoBehaviour {

	public Material colorJugador1;
	public Material colorJugador2;

	public void setColores(Material j1, Material j2){
		colorJugador1 = j1;
		colorJugador2 = j2;
	}

	public void PonerColorTurno(int turno){
		
		if(turno == 1)
			GetComponent<MeshRenderer> ().material = colorJugador1;
		else 
			GetComponent<MeshRenderer> ().material = colorJugador2;
	}

}
