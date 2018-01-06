/* UNIVERSIDADE FEDERAL DE UBERLANDIA
   Biomedical Engineering
   
   Autors: Ítalo G S Fernandes
           Julia Nepomuceno Mello
           
   contact: italogsfernandes@gmail.
   URLs: https://github.com/italogfernandes/

  Requisitos: Biblioteca Timer One[https://github.com/PaulStoffregen/TimerOne]
   
  Este codigo faz parte da disciplina de topicos avancados
  em instrumentacao boomedica e visa realizar a aquisicao
  de dados via o conversor AD do arduino e o envio destes
  para a interface serial.
  
  O seguinte pacote é enviado:
  Pacote: START | MSB  | LSB  | END
  Exemplo:  '$' | 0x01 | 0x42 | '\n'
*/

//Libraries
#include<TimerOne.h>

//Defines
#define PIN_SIGNAL    A0
#define PACKET_START  '$'
#define PACKET_END    '\n'

//Global Variables
uint16_t valor_lido;

//Setup
void setup() {
  Serial.begin(115200);
  Timer1.initialize(1000); //1000 microseconds betwween calls
  Timer1.attachInterrupt(doReading); // attach the service routine here
}

void loop() {

}

//Aquire Routine
void doReading() {
  valor_lido = analogRead(PIN_SIGNAL);
  sendData();
}

void sendData() {
  Serial.write(PACKET_START);
  Serial.write(valor_lido >> 8);
  Serial.write(valor_lido);
  Serial.write(PACKET_END);
}

