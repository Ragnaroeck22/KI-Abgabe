# KI-Prüfung Maximilian Röck
Dieses Projekt ist im Rahmen des Moduls "Künstliche Intelligenz" an der
SRH Hochschule Heidelberg entstanden. <br>
Ziel war es, eine KI zu entwickeln, die unter möglichst realistischen
Verhältnissen das Autofahren lernt. Mein persönliches Endziel war, dass
die KI außerdem lernt zu driften.<br>
Aus Zeitgründen wurden diese Ambitionen zurückgeschraubt. In dieser Version fährt ein 
sehr simples Auto über eine kurze Rennstrecke. 

## Anleitung zum Ausführen in Unity

Leider konnte ich das Projekt noch nicht auf einem anderen Gerät testen. Bestenfalls sollte
dieses Projekt out-of-the-box getestet werden können, ohne dass zusätzliche Abhängigkeiten 
bezogen werden müssen.

Dafür muss das Projekt zunächst im Unity Editor geöffnet werden. Es wurde in Unity
Version 2022.3.11f1 erstellt, ist wahrscheinlich aber mit neueren Versionen kompatibel.

Anschließend muss die "Demo Scene" geöffnet werden, welche unter Assets/Scenes zu finden ist.
Wird nun auf den Play-Button gedrückt, beginnt eine trainierte KI die Strecke zu durchqueren.

Führt dies zu Fehlern, sollte die folgende Anleitung ebenfalls befolgt werden.

## Anleitung für die Entwicklung

Python-Abhängigkeiten waren teils zu groß und führten zu Problemen mit 
GitHub, daher sind diese nicht im Repo enthalten und müssen selbst bezogen werden.


#### Wichtig: Aus mir unbekannten Gründen sind die Abhängigkeiten des Projekts nur mit sehr wenigen Versionen von Python kompatibel. Ich benutze Python 3.10.3.

Ich empfehle den im Repo exkludierte "Lib"-Ordner unter folgendem Link herunterzuladen:


Dieser soll im "MLvenv"-Ordner platziert werden

Sollte dies nicht möglich sein, können folgende Anweisungen befolgt werden. 
Es besteht kein Gewähr, dass diese Anleitung funktioniert, da ich die Schritte
nicht auf einem anderen Gerät testen konnte.


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

Nach Belieben kann auch eine mit Cuda kompatible version von PyTorch installiert werden, 
sofern zuvor Cuda installiert wurde.

Bei korrekter installation sollte der Befehl
``mlagents-learn -help``
eine Liste von Konfigurationsbefehlen ausgeben.



## Wichtige Commands für die Entwicklung

Dieses Kapitel hat nichts mit der Installation zu tun und dient lediglich als 
persönliche Gedankenstütze bei der Entwicklung.

### Runs speichern & beenden
#### Wichtig: Nie Alt + F4 bzw. Terminal schließen
Strg + C zum Speichern des aktuellen Runs

### Neuen ML Run starten
``
mlagents-learn --run-id =[Neue Run-ID]
``


### Existierenden ML Run fortsetzen
``
mlagents-learn --run-id =[Existierende Run-ID] --resume
``

### KI von anderem Model ableiten
``
mlagents-learn --initialize-from =[Run-ID] --run-id =[Neue Run-ID] 
``


### Oft verwendete Befehle:
```
mlagents-learn config/sCarIL.yaml --run-id =CarILRoundTrack --time-scale 1
mlagents-learn config/sCarIL.yaml --initialize-from =[Run-ID] --run-id =CarILRoundTrack --time-scale 1
```

### Parameter

SCarComplexCourse: Ex: 1.0, GAIL: 0.01, BC: 1.0

SCCCnp: Ex: 1.0, GAIL: 0.3, BC: 0.7

SCAvoidWalls (abgeleitet von SCCCnp3): Ex: 1.0, GAIL: 0, BC: 0