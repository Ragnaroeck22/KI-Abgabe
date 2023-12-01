# KI-Prüfung Maximilian Röck
Dieses Projekt ist im Rahmen des Moduls "Künstliche Intelligenz" an der
SRH Hochschule Heidelberg entstanden. <br>
Ziel war es, eine KI zu entwickeln, die unter möglichst realistischen
Verhältnissen das Autofahren lernt. Mein persönliches Endziel war, dass
die KI außerdem lernt zu driften.

## Anleitung zum Ausführen in Unity

Python-Abhängigkeiten waren teils zu groß und führten zu Problemen mit 
GitHub, daher müssen diese selbst bezogen werden.


#### Aus mir unbekannten Gründen sind die Abhängigkeiten des Projekts nur mit sehr wenigen Versionen von Python kompatibel. Ich benutze Python 3.10.3.

Zunächst muss das Terminal geöffnet und der Dateipfad des
Projekts ausgewählt werden, bspw. mit folgendem Befehl:

``
cd F:\Unity\KI Abgabe
``

Danach muss das Python virtual environment aktiviert werden.

``
MLvenv\Scripts\activate.bat
``

Schließlich werden die Abhängigkeiten installiert. Folgende Befehle müssen
nacheinander ausgeführt werden:

```
pip3 install mlagents
pip3 install torch torchvision torchaudio
```

Bei korrekter installation sollte 
``mlagents-learn -h``
eine Liste von Konfigurationsbefehlen ausgeben.



## Wichtige Commands für die Entwicklung

### Runs speichern & beenden
#### Wichtig: Nie Alt + F4 bzw. Terminal schließen
Strg + C zum Speichern des aktuellen Runs

### Neuen ML Run starten
``
mlagents-learn --run-id =[Neue Run-ID]
``

#### Beispiel:
``
mlagents-learn --run-id =PelletGrabberRun3
``

### Existierenden ML Run fortsetzen
``
mlagents-learn --run-id =[Existierende Run-ID] --resume
``

#### Beispiel:
``
mlagents-learn --run-id =PelletGrabberRun3 --resume
``

### Aktueller Befehl:
``
mlagents-learn --run-id =CarFirstRun --time-scale 1
``

``
mlagents-learn --run-id =CarFirstRun --resume --time-scale 1
``