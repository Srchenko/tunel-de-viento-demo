using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fuzzy_script : MonoBehaviour
{
    public Text texto2;
    public Text texto3;
    public Text texto4;
    private float gravedad = 9.81f;
    private float velocidad_y;
    private float posicion_y;
    private float velocidad_ventilador;
    private float altura_objetivo = 540;

    private Transform objeto;

    void Start()
    {
        objeto = gameObject.transform;
        objeto.position += new Vector3(0, Random.Range(-100.0f, 100.0f), 0);
        posicion_y = objeto.position.y;
        Debug.Log(gameObject.transform.position);
    }
    private float Grado(float x, float y, float z) {
        float resultado = 0;
        if (x <= y) {
            resultado = 0;
        } else {
            if(x > y && x < z){
                resultado = (x/(z-y))-(y/(z-y));
            }else{
                if(x>= z){
                    resultado = 1;
                }
            }
        }
        return resultado;
    }

    private float GradoInvertido(float x, float y, float z) {
        float resultado = 0;
        if (x <= y) {
            resultado = 1;
        } else {
            if(x > y && x < z){
                resultado = (x/(z-y))-(z/(z-y));
            }else{
                if(x>= z){
                    resultado = 0;
                }
            }
        }
        return resultado;
    }

    private float Triangulo(float x, float a, float b, float c){
        float resultado = 0;
        if(x <= a) {
            resultado = 0;
        }else{
            if (x > a && x <= b) {
                resultado = (x/(b-a)) - (a/(b-a));
            }else{
                if (x > b && x <= c) {
                    resultado = - (x/(c-b)) + (c/(c-b));
                } else {
                    if (x>c) {
                        resultado = 0;
                    }
                }
            }
        }
        return resultado;
    }

    private float Trapezoide(float x, float a, float b, float c, float d){
        float resultado = 0;
        if(x <= a) {
            resultado = 0;
        }else{
            if (x > a && x <= b) {
                resultado = (x/(b-a)) - (a/(b-a));
            }else{
                if (x > b && x <= c) {
                    resultado = 1;
                } else {
                    if (x>c && x <= d) {
                        resultado = - (x/(d-c)) + (d/(d-c));
                    } else {
                        if (x>d) {
                            resultado = 0;
                        }
                    }
                }
            }
        }
        return resultado;
    }

    private void LogicaDifusa(){
        float distancia = altura_objetivo - posicion_y;

        float centrado = Triangulo(distancia, -40, 0, 40);

        float cercaA = Trapezoide(distancia, 20, 80, 120, 180);
        float normalA = Trapezoide(distancia, 120, 160, 240, 280);
        float lejosA = Grado(distancia, 240, 300);

        float cercaB = Trapezoide(distancia, -180, -120, -80, -20);
        float normalB = Trapezoide(distancia, -280, -240, -160, -120);
        float lejosB = GradoInvertido(distancia, -300, -240);

        float numerador = centrado*9.8f + cercaA*4 + normalA*2 + lejosA*1 + cercaB*14 + normalB*15.5f + lejosB*18;
        float denominador = centrado+cercaA+normalA+lejosA+cercaB+normalB+lejosB;

        velocidad_ventilador = numerador/denominador;
    }

    private void ActualizarTextos(){
        texto2.text = "Posición Y: " + objeto.position.y;
        texto3.text = "Velocidad Y: " + velocidad_y;
        texto4.text = "Ventilador: " + velocidad_ventilador;
    }

    void Update()
    {
        LogicaDifusa();
        float caos = Random.Range(-5.0f, 5.0f);
        velocidad_y += (gravedad - velocidad_ventilador + caos) * 0.01f;
        posicion_y += velocidad_y;
        objeto.position += new Vector3(0, velocidad_y, 0);
        ActualizarTextos();
    }
}
