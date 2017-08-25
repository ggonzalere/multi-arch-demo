#!/bin/sh
if ! type docker  >/dev/null 2>&1 ; then
  echo "Install docker from docker.com"
  echo "curl -L https://get.docker.com | sh"
  curl -L https://get.docker.com | sh
fi

#Get the registry and application details from the user.

read -p 'Login URL: ' LOGIN_URL
printf "Image name $LOGIN_URL/[image] "
read APP_NAME

IMAGE="$LOGIN_URL/$APP_NAME"

#Docker login into registry 
#Check if you need to login
echo Trying to pull image,
if  echo "$(docker pull $IMAGE 2>&1)" | grep -q  "not found" ; then
  echo Could not pull image. Trying to login again. 
  docker login $LOGIN_URL
fi

#Declare container ID
CID=""

get_container_id()
{
    #If docker build is tagging then we don't get the running container
    #so we try again after a few seconds to get the container id again. 
    CID=$(docker ps | grep $IMAGE| awk '{print $1}')   
    if [ -z "$CID" ]; then
      sleep 5 
      CID=$(docker ps | grep $IMAGE| awk '{print $1}') 
    fi 
}


watch_for_updates()
{
  echo Pulling Latest $IMAGE

  #Update container ID 
  get_container_id

  if [ -z "$CID" ]; then
     echo "No container found for $IMAGE."
     echo "Launching container..."
     docker run -d $IMAGE
     return;
  fi

  docker pull $IMAGE
  for im in $CID
  do
    NC='\033[0m' # No Color
    YELLOW='\033[0;33m'
    LATEST=`docker inspect --format "{{.Id}}" $IMAGE`
    RUNNING=`docker inspect --format "{{.Image}}" $im`
    NAME=`docker inspect --format '{{.Name}}' $im | sed "s/\///g"`
    echo -e "${NC}Latest : " $LATEST 
    echo -e "${NC}Running: " $RUNNING
    if [ "$RUNNING" != "$LATEST" ];then
        #We are upgrading the container                   
        echo -e "${YELLOW}Upgrading $NAME "
        echo "Stopping Container" 
	      docker stop $NAME
        docker rm -f $NAME
        
	      echo "Starting $IMAGE ..."
	      docker run -d --name $NAME $IMAGE >/dev/null
        echo -e "${NC}..."
    fi
  done
}


#Every 10 seconds check for updates 
while true
do
   watch_for_updates
   sleep 10
done 
