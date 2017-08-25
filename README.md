# Multi-architecture container demo.
## Build
First we need to build all the containers.
 * Java: The demo uses two containers written in java for the raspberry pi.
```bash
cd Raspberry/Celsius
docker build -t "your_image:tag1" .
cd ..
cd Fahrenheit
docker build -t "your_image:tag2" .
```

* Linux: From root:
```bash
cd WeatherLinuxF/WeatherWindowsF/
docker build -t "your_image:tag3" .
```

* Windows: From root:
```bash
cd WeatherWindowsF/WeatherWindowsF/
docker build -t "your_image:tag4" .
```
* API:
```bash
cd WeatherRecopiler/WeatherRecopiler/
docker build -t "your_image:tag5" .
```

To test the simple multi-architecture feature just push the three images to an ACR and create the multi-architecture flow either from portal or from CLI.

To test the autoupdate feature run the following script, it will constantly pull from the latest version of an image you specify.
```bash
cd Raspberry
chmod +x device-setup.sh
sh device-setup.sh
```
In another terminal run:
```bash
watch -n1 'docker ps -q --filter ancestor="your_image" | xargs -n1 docker logs -t'
```

### Links:

 * [ACR-Get](https://github.com/SajayAntony/acr-get) for more information about the device-setup.sh