using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour {

	public Material colorCasilla;
	public int numCasilla;
	bool movimientoPosible;
	int estado;
	int fila;
	int columna;

	bool presionada = false;

	void OnMouseDown(){
		presionada = true;
	}

	public bool getPresionada(){
		return presionada;
	}

	public void NoPresionar() {
		presionada = false;
	}
	public void AsignarNumeroCasilla(int numero){
		numCasilla = numero;
	}

	public void setPosicion(int fila_, int columna_){
		fila = fila_; columna = columna_;
	}

	public void setEstado(int x){
		estado = x;
	}

	public int getEstado(){
		return estado;
	}

	public void setMovimientoPosible(bool movP){
		movimientoPosible = movP;
	}

	public bool isMovimientoPosible(){
		return movimientoPosible;
	}
		

	public int getFila(){
		return fila;
	}

	public int getColu(){
		return columna;
	}
		
	public void PonerColorFicha(Material colorFicha){
		GetComponent<MeshRenderer> ().material = colorFicha;
		colorCasilla = colorFicha; 
		presionada = false;
	}
}
