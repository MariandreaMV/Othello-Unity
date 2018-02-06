using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearCasillas : MonoBehaviour {

	//variables publicas
	public GameObject casillasTablero;
	public GameObject JTurno;
	List <GameObject> tablero = new List<GameObject>();
	public Material Jugador1;
	public Material Jugador2;
	public Material vacio;
	public Material posibleJ;
	public int ancho;
	public int alto;

	//variables privadas, solo para mi control
	int turno = 1;
	int tam = 8;
	int contCasillas = 0;
	GameObject colorJugadorActual;//sphere
	GameObject[,] tab = new GameObject[8, 8];//matriz cuadritos, tablero
	bool inicio = true;
	bool pistas = false;
	bool IA = false;
	GameObject JugadorTurno;
	int numeroMovimientosPosibles = 0;

	public void InteligenciaArtificial(){
		IA = true;
			Reiniciar ();
	}

	public void HvsH(){
		IA = false;
			Reiniciar ();
	}

	public void Pistas(){
		
		if (pistas)
			pistas = false;
		else pistas = true;

		if(contCasillas!=0)
		pintarPosibleJugadas();
	}

	//solo se ejecuta al presionar el boton iniciar partida.
	public void Start(){

		if (inicio == true){
			
			 JugadorTurno = Instantiate (JTurno, new Vector3(6,-1.5f,1),Quaternion.identity);

			for(int x=0 ; x<ancho; x++){	
					
				for(int y=0; y<alto; y++){
					
					GameObject casillaCreada = Instantiate (casillasTablero, new Vector3(x-1f,y+0.01f,1), Quaternion.identity);//instancio el objeto	

					casillaCreada.GetComponent<Casilla> ().setEstado (0); //asigno estado de la casilla

					if (inicio == true && (contCasillas == 28 || contCasillas == 35)) {//posiciones centrales para inicializar partida
						casillaCreada.GetComponent<Casilla> ().PonerColorFicha (Jugador1);
						casillaCreada.GetComponent<Casilla>().setEstado(1);//posicion inicial del jugador 1
					}

					if (inicio == true && (contCasillas == 27 || contCasillas == 36)) {//posiciones centrales para inicializar partida
						casillaCreada.GetComponent<Casilla> ().PonerColorFicha (Jugador2);
						casillaCreada.GetComponent<Casilla>().setEstado (2);//posicion inicial del jugador 2
					}
	
					casillaCreada.GetComponent<Casilla>().setPosicion(x,y); // ubicacion matricial
					casillaCreada.GetComponent<Casilla>().setMovimientoPosible(false); //no es movimiento posible aun
					casillaCreada.GetComponent<Casilla>().AsignarNumeroCasilla (contCasillas);// id de la casilla
					tablero.Add (casillaCreada); // guardo la casilla en el array tablero para luego evaluar juego
					tab[x,y]= casillaCreada;//guardo el objeto en la matriz

					contCasillas++;// contador de casillas, para asignar id
				}
			}
			setMovimientosPosibles ();//movimientos posibles a turno 1
			pintarPosibleJugadas();//pinta posibles jugadas;
			inicio = false;//ya no esta iniciando partida.
		} else Reiniciar();// si no se reinician los valores de cada casilla para reiniciar

		colorJugadorActual = JugadorTurno;
		//JugadorTurno.GetComponent<Turno> ().setColores (Jugador1,Jugador2);
		JugadorTurno.GetComponent<Turno> ().PonerColorTurno (turno);

	}//fin crear



	public void cambiarTurno(){
		if (turno == 1)
			turno = 2;
		else
			turno = 1;
		setMovimientosPosibles ();//asigno los movimientos posibles al siguiente turno
		colorJugadorActual.GetComponent<Turno> ().PonerColorTurno (turno);
		pintarPosibleJugadas();
	}//fin cambiar turno
		


	public void Reiniciar(){

		Start ();

		GameObject c;

		for (int x = 0; x < 64; x++) {
			
			c = tablero [x];
			c.GetComponent<Casilla>().setMovimientoPosible(false); //no es movimiento posible aun

			if (x == 28 || x == 35) {
				c.GetComponent<Casilla> ().PonerColorFicha (Jugador1);
				c.GetComponent<Casilla>().setEstado(1);//posicion inicial del jugador 1
			}else if (x == 27 || x == 36) {
				c.GetComponent<Casilla> ().PonerColorFicha (Jugador2);
				c.GetComponent<Casilla>().setEstado (2);//posicion inicial del jugador 2
			} else{ 
				c.GetComponent<Casilla> ().PonerColorFicha (vacio);
				c.GetComponent<Casilla> ().setEstado (0); //asigno estado de la casilla
			}

		}

		turno = 1;
		setMovimientosPosibles ();
		//pintarPosibleJugadas();
		colorJugadorActual.GetComponent<Turno> ().PonerColorTurno (turno);
	}//fin reiniciar


	public void setMovimientosPosibles(){

		bool enemigoEncontrado = false;
		int movimientosPosibles = 0; 
		int enemigo;
		int estadoCasillaActual;

		if(turno == 1)
			enemigo = 2;
		else
			enemigo = 1;

		//limpia los estados anteriores.
		for(int x=0;x<tam;x++){
			for (int y = 0; y < tam; y++) {
				tab[x,y].GetComponent<Casilla>().setMovimientoPosible(false);
			}
		}


		for(int x=0;x<tam;x++){
			for (int y = 0; y < tam; y++) {
				
				enemigoEncontrado = false;
				estadoCasillaActual = tab[x,y].GetComponent<Casilla> ().getEstado ();

				while(estadoCasillaActual!=0){
					
					y++;
					if (y < tam) {
						estadoCasillaActual = tab [x, y].GetComponent<Casilla> ().getEstado ();
					} else {
						y = 0;
						x++;
						if (x >= tam) 
							return;
						 else 
							estadoCasillaActual = tab [x, y].GetComponent<Casilla> ().getEstado ();
						
					}
				}

				//casilla de la izquierda
				for(int k= y-1 ; k>=0 ; k--){
					if (tab [x, k].GetComponent<Casilla> ().getEstado () == enemigo)
						enemigoEncontrado = true;
					else {
						if(enemigoEncontrado && tab[x,k].GetComponent<Casilla>().getEstado() == turno){
							tab[x,y].GetComponent<Casilla>().setMovimientoPosible(true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
				}
				enemigoEncontrado = false;

				//casilla de la derecha
				for(int k= y+1 ; k<tam ; k++){
					if (tab [x, k].GetComponent<Casilla> ().getEstado () == enemigo)
						enemigoEncontrado = true;
					else {
						if(enemigoEncontrado && tab[x,k].GetComponent<Casilla>().getEstado() == turno){
							tab[x,y].GetComponent<Casilla>().setMovimientoPosible(true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
				}
				enemigoEncontrado = false;

				//casillas de arriba
				for(int k= x-1 ; k>= 0; k--){
					if (tab [k, y].GetComponent<Casilla> ().getEstado () == enemigo)
						enemigoEncontrado = true;
					else {
						if(enemigoEncontrado && tab[k,y].GetComponent<Casilla>().getEstado() == turno){
							tab[x,y].GetComponent<Casilla>().setMovimientoPosible(true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
				}
				enemigoEncontrado = false;

				//casillas de abajo
				for(int k= x+1 ; k<tam; k++){
					if (tab [k, y].GetComponent<Casilla> ().getEstado () == enemigo)
						enemigoEncontrado = true;
					else {
						if(enemigoEncontrado && tab[k,y].GetComponent<Casilla>().getEstado() == turno){
							tab[x,y].GetComponent<Casilla>().setMovimientoPosible(true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
				}
				enemigoEncontrado = false;

				int r, c;

				//diagonal superior izquierda
				r = x - 1;
				c = y - 1;
				while( r>= 0 && c>=0){

					if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
						enemigoEncontrado = true;
					} else {
						if(enemigoEncontrado && tab[r,c].GetComponent<Casilla>().getEstado()== turno){
							tab [x, y].GetComponent<Casilla> ().setMovimientoPosible (true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
					r--;
					c--;
				}

				enemigoEncontrado = false;

				//diagonal superior derecha
				r = x - 1;
				c = y + 1;
				while( r>= 0 && c<tam){

					if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
						enemigoEncontrado = true;
					} else {
						if(enemigoEncontrado && tab[r,c].GetComponent<Casilla>().getEstado()== turno){
							tab [x, y].GetComponent<Casilla> ().setMovimientoPosible (true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
					r--;
					c++;
				}

				enemigoEncontrado = false;

				//diagonal inferior izquierda
				r = x + 1;
				c = y - 1;
				while( r<tam && c>=0){

					if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
						enemigoEncontrado = true;
					} else {
						if(enemigoEncontrado && tab[r,c].GetComponent<Casilla>().getEstado()== turno){
							tab [x, y].GetComponent<Casilla> ().setMovimientoPosible (true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
					r++;
					c--;
				}

				enemigoEncontrado = false;

				//diagonal inferior derecha
				r = x + 1;
				c = y + 1;
				while( r < tam && c < tam ){

					if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
						enemigoEncontrado = true;
					} else {
						if(enemigoEncontrado && tab[r,c].GetComponent<Casilla>().getEstado()== turno){
							tab [x, y].GetComponent<Casilla> ().setMovimientoPosible (true);
							movimientosPosibles++;
						}
						enemigoEncontrado = false;
						break;
					}
					r++;
					c++;
				}
			}
		}

		numeroMovimientosPosibles = movimientosPosibles;

	}//fin setMovimientosPosibles


	public void Jugar(int fila, int colu){
		
		bool enemigoEncontrado = false;
		int enemigo;

		if (turno == 1)
			enemigo = 2;
		else
			enemigo = 1;

		for(int k = colu -1 ; k>= 0 ; k-- ){
			if (tab [fila, k].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[fila,k].GetComponent<Casilla>().getEstado()== turno)
					while(k+1 <= colu){
						k++;
						tab [fila, k].GetComponent<Casilla> ().setEstado (turno);//creo que aqui pinto
					}
				break;
				}
		}//fin for

		enemigoEncontrado = false;

		for(int k = colu +1 ; k<tam ; k++ ){
			if (tab [fila, k].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[fila,k].GetComponent<Casilla>().getEstado()== turno)
					while(k-1 >= colu){
						k--;
						tab [fila, k].GetComponent<Casilla> ().setEstado (turno);//creo que aqui pinto
					}
				break;
			}
		}//fin for

		enemigoEncontrado = false;

		for(int k = fila -1 ; k>= 0 ; k-- ){
			if (tab [k, colu].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[k,colu].GetComponent<Casilla>().getEstado()== turno)
					while(k+1 <= fila){
						k++;
						tab [k, colu].GetComponent<Casilla> ().setEstado (turno);//creo que aqui pinto
					}
				break;
			}
		}//fin for

		enemigoEncontrado = false;


		for(int k = fila +1 ; k<tam ; k++ ){
			if (tab [k, colu].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[k,colu].GetComponent<Casilla>().getEstado()== turno)
					while(k-1 >= fila){
						k--;
						tab [k, colu].GetComponent<Casilla> ().setEstado (turno);//creo que aqui pinto
					}
				break;
			}
		}//fin for

		enemigoEncontrado = false;

		int r, c;
	
		r = fila - 1;
		c = colu - 1;

		while(r>=0 && c>=0){
			if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[r,c].GetComponent<Casilla> ().getEstado()==turno){
					while((r+1)<= fila && (c+1)<= colu){
						r++; c++;
						tab [r, c].GetComponent<Casilla> ().setEstado (turno);
					}
				}
				break;
			}
			r--; c--;
		}

		enemigoEncontrado = false;
	

		r = fila - 1;
		c = colu + 1;

		while(r>=0 && c<tam){
			if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[r,c].GetComponent<Casilla> ().getEstado()==turno){
					while((r+1)<= fila && (c-1)>= colu){
						r++; c--;
						tab [r, c].GetComponent<Casilla> ().setEstado (turno);
					}
				}
				break;
			}
			r--; c++;
		}

		enemigoEncontrado = false;


		r = fila + 1;
		c = colu - 1;

		while(r<tam && c>=0){
			if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[r,c].GetComponent<Casilla> ().getEstado()==turno){
					while((r-1)>= fila && (c+1)<= colu){
						r--; c++;
						tab [r, c].GetComponent<Casilla> ().setEstado (turno);
					}
				}
				break;
			}
			r++; c--;
		}

		enemigoEncontrado = false;
	
		r = fila + 1;
		c = colu + 1;

		while(r<tam && c<tam){
			if (tab [r, c].GetComponent<Casilla> ().getEstado () == enemigo) {
				enemigoEncontrado = true;
			} else {
				if(enemigoEncontrado && tab[r,c].GetComponent<Casilla> ().getEstado()==turno){
					while((r-1)>= fila && (c-1)>= colu){
						r--; c--;
						tab [r, c].GetComponent<Casilla> ().setEstado (turno);
					}
				}
				break;
			}
			r++; c++;
		}

	}//fin jugar


	public void pintar(){
	
		GameObject c;

		for(int x=0; x<tam;x++){
			for(int y=0;y<tam;y++){
				c = tab [x, y];
				int estado = c.GetComponent<Casilla> ().getEstado ();
				if (estado == 1) {
					c.GetComponent<Casilla> ().PonerColorFicha (Jugador1);
				} else if(estado == 2){
					c.GetComponent<Casilla> ().PonerColorFicha (Jugador2);
				}else 
					c.GetComponent<Casilla> ().PonerColorFicha (vacio);
			}
		}
	
	}//fin pintar



	public void pintarPosibleJugadas (){
		
		GameObject c;
		if(pistas)
		for(int x=0; x<tam;x++){
			for (int y = 0; y < tam; y++) {
				c = tab [x, y];
				bool jposible = c.GetComponent<Casilla> ().isMovimientoPosible ();
				if (jposible) {
					c.GetComponent<Casilla> ().PonerColorFicha (posibleJ);
				}
			}
		}
		else
			for(int x=0; x<tam;x++){
				for (int y = 0; y < tam; y++) {
					c = tab [x, y];
					bool jposible = c.GetComponent<Casilla> ().isMovimientoPosible ();
					if (jposible) {
						c.GetComponent<Casilla> ().PonerColorFicha (vacio);
					}
				}
			}

	}//fin pintarPosiblesJugadas



	public GameObject Inteligencia(){


		int jugada = Random.Range (0,numeroMovimientosPosibles);
		int contVeces = 0;

		for(int x=0; x<tam;x++){
			for (int y = 0; y < tam; y++) {

				if (tab [x, y].GetComponent<Casilla> ().isMovimientoPosible ()) {
					if (jugada == contVeces) {
						return tab [x, y];
					} 
					contVeces++;
				}
			}
		}

		return null;

	}






	//ciclo que se realiza constantemente
	void Update(){

		//solo voy a evaluar si ya inicio una partida.
		if (inicio == false) {
			GameObject c;

			for(int a=0; a<64;a++){
				c = tablero [a];

				if (c.GetComponent<Casilla> ().getPresionada () ) {
					if(c.GetComponent<Casilla> ().isMovimientoPosible()){
						
						Jugar(c.GetComponent<Casilla> ().getFila(),c.GetComponent<Casilla> ().getColu());
						pintar ();
						cambiarTurno();

						if (IA) {
							c = Inteligencia();
							if (c) {
								Jugar (c.GetComponent<Casilla> ().getFila (), c.GetComponent<Casilla> ().getColu ());
								pintar ();
								cambiarTurno ();
							} else {
								print ("pierde turno");
								cambiarTurno ();
							}

						}
							
						break;
						//------------termino el proceso de cambiar estado de la casilla
					}else{
						print ("movimiento no valido");
					}

				}//fin presionada
			}
		}


	}//fin update

	public void DestroyGameObject()
	{
		foreach (GameObject o in GameObject.FindGameObjectsWithTag("Clone")) {
			Destroy (o);
		}
	}

}
