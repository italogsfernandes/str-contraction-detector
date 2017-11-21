/* UNIVERSIDADE FEDERAL DE UBERLANDIA
   Biomedical Engineering
   Autors: Ítalo G S Fernandes
   contact: italogsfernandes@gmail.com
   URLs: https://github.com/italogfernandes/str
  Description
*/

////////////
//Defines //
////////////
//#define DEBUG_MODE

#define UART_BAUDRATE 115200
#define PINO_ADC A0
#define UART_START '$'
#define UART_END '\n'

#define AQUIRE_FREQ 75
///////////
//Clases //
///////////

/////////////////////////////////////////////////
// Implementacao de um timer atraves do millis //
/////////////////////////////////////////////////

class Timer {
  private:
    unsigned long _actual_time;
    unsigned long _waited_time;
    bool _running;
    uint32_t _interval;

  public:
    bool elapsed;

    Timer(uint32_t interval = 1000) {
      _interval = interval;
      elapsed = false;
      _actual_time = micros();
      _waited_time = _actual_time + interval;
    }

    void setInterval(uint32_t interval) {
      _interval = interval;
    }

    uint32_t getInterval() {
      return _interval;
    }

    void start() {
      _running = true;
      _actual_time = millis();
      _waited_time = _actual_time + _interval;
    }

    void stop() {
      _running = false;
    }

    void wait_next() {
      elapsed = false;
    }

    void setFreq(uint32_t frequency) {
      setInterval(1000000 / frequency);
    }

    uint32_t getFreq() {
      return 1000000 / getInterval();
    }

    void update() {
      _actual_time = micros();
      if (_running) {
        if (_actual_time >= _waited_time) {
          _waited_time = _actual_time + _interval;
          elapsed = true;
        }
      }
    }

};

Timer aquisition;
uint16_t valor_adc;
void setup() {
  Serial.begin(UART_BAUDRATE);
  aquisition.setFreq(AQUIRE_FREQ); //100Hz
  aquisition.start();
}
void loop() {
  aquisition.update();
  if (aquisition.elapsed) {
    //doAquire();
    emulateRampa();
    aquisition.wait_next();
  }
}

void doAquire() {
  valor_adc = analogRead(PINO_ADC);
#ifdef DEBUG_MODE
  Serial.println(valor_adc);
#endif
#ifndef DEBUG_MODE
  Serial.write(UART_START);
  Serial.write((uint8_t) (valor_adc >> 8));
  Serial.write((uint8_t) (valor_adc & 0xFF));
  Serial.write(UART_END);
#endif
}

void emulateRampa() {
  valor_adc += 10;
  if(valor_adc > 1000){
    valor_adc = 0;
  }
#ifdef DEBUG_MODE
  Serial.println(valor_adc);
#endif
#ifndef DEBUG_MODE
  Serial.write(UART_START);
  Serial.write((uint8_t) (valor_adc >> 8));
  Serial.write((uint8_t) (valor_adc & 0xFF));
  Serial.write(UART_END);
#endif
}

