
# Code-evaluation-system

## Table of Contents
  - [Judge0](#Judge0setup)
  - [Postman](#Postman)


## Judge0setup

<pre>
в windows търсачката -> turn windows features on or off  
чекваме hyper-v  
(ако липсва като опция:  
pushd "%~dp0"  
dir /b %SystemRoot%\servicing\Packages*Hyper-V*.mum >hyper-v.txt  
for /f %%i in ('findstr /i . hyper-v.txt 2^>nul') do dism /online /add-package:"%SystemRoot%\servicing\Packages%%i"  
del hyper-v.txt  
Dism /online /enable-feature /featurename:Microsoft-Hyper-V-All /LimitAccess /ALL  
pause  
(в notepad и после на .bat file)  

Даваме рестарт на системата.  

Отваряме Hyper-V Manager и в ляво виждаме машината си.  
Right click - new Virtual Machine  
Specify Generation - first generation  
Assign memory - 4096 (би трябвало да стига)  
Configure networking - default switch  
Connect virtual hard disk - 40gb  
Installation options - Install an operating system from bootable cd/dvd-rom чек и Image file(iso) чек.
</pre>
Теглим https://ubuntu.com/download/desktop и го избираме.
<pre>
Finish.  

Double click за да пуснем vm-a и enter -> try or install ubuntu.  
В ubuntu install wizard-a само next, няма нужда да променяме нищо и си правим админ акаунта.  
Изчакваме инсталацията и рестарт когато е готово(може да излязат грешки в конзолата, но не е проблем за момента).  

Пускаме отново vm-a и би трябвало вече да сме с инсталирано ubuntu 24.04  


Отваряме си терминал и започваме:  

sudo nano /etc/default/grub  
в кавичките на GRUB_CMDLINE_LINUX слагаме systemd.unified_cgroup_hierarchy=0  
ctrl + x > y > enter за да запазим промените  
sudo update-grub  
sudo reboot  

След рестарта отново отваряме терминал и продължаваме:  

sudo apt remove docker docker-engine docker.io containerd runc  
sudo apt update  
sudo apt install ca-certificates curl gnupg  
sudo install -m 0755 -d /etc/apt/keyrings  
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg  
sudo chmod a+r /etc/apt/keyrings/docker.gpg  
echo \ "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] \ 
  https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo $VERSION_CODENAME) stable" \ 
  | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null  
sudo apt update  
sudo apt install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin  
sudo systemctl enable --now docker  
sudo usermod -aG docker $USER  
newgrp docker  
Ако ни поиска рестарт/ъпдейт се съгласяваме и продължаваме със стъпките след това.  
Можем да тестваме дали за момента нещата са наред с docker run  hello-world и docker compose version.  

Продължаваме:  
cd /  
ip a  
Търсим inet при eth0 и си записваме ip-то някъде, което ще ни трябва в бъдеще. (това което е последвано от /20)   

Продължаваме:  
sudo mkdir judge0  
cd judge0  
sudo wget https://github.com/judge0/judge0/releases/download/v1.13.1/judge0-v1.13.1.zip  
sudo unzip judge0-v1.13.1.zip  
cd judge0-v1.13.1  
sudo nano judge0.conf  
Търсим REDIS_PASSWORD и POSTGRES_PASSWORD и им даваме някакви стойности, няма значение какви.  
ctrl+x > y > enter  
docker compose up -d db redis (изчакваме да приключи)  
docker compose up -d (изчакваме да приключи)  
Ако забие/трябва да рестартираме тук:  
cd /judge0  
cd judge0-v1.13.1  
И отновно рънваме docker compose up -d db redis или docker compose up -d, според това до къде сме стигнали.</pre>

Когато приключи би трябвало да сме готови.  
Във vm-а judge0 e на http://localhost:2358  
В windows машинати е на [ip-то което си запазихме някъде преди малко]:2358  
Простичък тест е да напраим crul http://localhost:2358/languages или [ip-то]:2358/languages в терминал.  

## Postman
Информация за най-основните API calls в judge0
