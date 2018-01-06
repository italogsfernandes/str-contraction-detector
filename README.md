# STR - Contraction Detector
> Projetor da disciplina de STR e um detector de contrações em tempo real.

Neste repositório estão todos os projetos desenvolvidos ao longo da diciplina TAIB, assim como um detector de contrações. Este detector aquire dados de um emg com o arduino, os envia para o computador pela porta serial, processa estes dados através da transformada de hilbert e
um filtro passa-baixa do sinal retificado para obter a envoltória. Com esta envoltória detecta se está ou não em contração.

![](docs/screenshot.jpg)

## Neste repositório

- Docs: Imagens, guias, referências, códigos consultados.
- Projetos:
    - 1_Paint:
    - 2_Pong:
    - 3_SignalGenerator:
    - 4_TAIB_Thead1:
    - 5_ArduinoPlotter:
- ContractionDetector:


## Development setup

* [Visual Studio](https://www.visualstudio.com/downloads/)
* [Libraries](https://github.com/italogfernandes/libraries) - All the used custom libraries could be found here, if you didn't found someone please contact me.
* [Python3](https://www.python.org/downloads/) with the following modules installed:
    * pyserial
    * numpy
    * scipy
    * pyqtgraph
    * PyQt4, PyQt5

* Arduino:
  *  [Platomformio](https://atom.io/packages/platomformio) - Atom integration with PlatformIO (for building arduino files,but arduino IDE can also be used.).
  * [Arduino IDE](www.arduino.cc) - If you prefer.

## Warning: Undocumented codes!!!
I will comment it later... there is almost no PEP8 conventions here.

## More Info
Estes são os primeiros links que achei quando joguei a ideia no google:
* [Wizard's Chess](http://poe.olin.edu/poe2014/knights/): The same ideia of this project.
* [ChessInfo](https://pino.github.io/chessinfo.html): Responsive chessboard with integrated game logic and speech recognition.
* [JabberChess](https://cmusphinx.github.io/data/chess/ExtendedEssay.pdf): A Mobile Chess App Speech Recognizer for Visual and Motion Impaired Users
* [Voice Activated Chess Set](http://ideaexchange.uakron.edu/cgi/viewcontent.cgi?article=1356&context=honors_research_projects)
* [A nice video](https://www.youtube.com/watch?v=QEGJUZoUTCE): Voice Operated Chess Board
*  [Another Video](https://www.youtube.com/watch?v=dX37LFv8jWY): Wireless Arduino Powered Chess

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

```
"THE BEERWARE LICENSE" (Revision 42):
Italo Fernandes wrote this code. As long as you retain this
notice, you can do whatever you want with this stuff. If we
meet someday, and you think this stuff is worth it, you can
buy me a beer in return.
```
## Authors

* **Italo Fernandes** - https://github.com/italogfernandes - italogsfernandes@gmail.com

See also the list of [contributors](https://github.com/italogfernandes/mtp-xadrez-de-bruxo/contributors) who participated in this project.

[https://github.com/italogfernandes/mtp-xadrez-de-bruxo](https://github.com/italogfernandes/mtp-xadrez-de-bruxo)
