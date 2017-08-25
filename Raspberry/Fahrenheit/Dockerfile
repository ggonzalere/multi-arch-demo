FROM resin/raspberry-pi-openjdk
ADD . /home
WORKDIR /home/WeatherRaspberry/src
RUN javac -cp .:gson-2.8.1.jar WeatherRaspberry.java
CMD java -cp .:gson-2.8.1.jar WeatherRaspberry
