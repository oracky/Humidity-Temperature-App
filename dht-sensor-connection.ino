#include <Timers.h>
#include <DHT.h>
#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>


#define DHT11_PIN 13
#define DHTTYPE DHT11
#define INSECURE_MODE     //if there is a problem with conection to google script and security is not necessary


String readString;
const char* ssid = "##########";  //your ssid
const char* password = "#########"; //your wifi password
const char* host = "script.google.com";  
const int httpsPort = 443;



WiFiClientSecure client;

const char* fingerprint = "46 B2 C3 44 9C 59 09 8B 01 B6 F8 BD 4C FB 00 74 91 2F EF F6";
String GAS_ID = "###########";   //google script ID


struct sensorData
{
    unsigned int humidity;
    unsigned int tempCelsius;
    unsigned int tempFarenheit;
    unsigned int hindexC;  // in Celsius
    unsigned int hindexF;  // in Farenheit
};

DHT dht(DHT11_PIN,DHTTYPE);
Timer dhtSensorTimer;

void setup() {
  
  Serial.begin(115200);
  Serial.println();
  
  Serial.print("connecting to ");  //starting connection
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  
  pinMode(DHT11_PIN,OUTPUT);  //setting up the sensor
  dht.begin();
  dhtSensorTimer.begin(MINS(10));  // measurement every 10 minutes 
  
}

void loop() {

   


 if(dhtSensorTimer.available())
 {
     sensorData sensor;

     sensor = readSensorData();     

     if (isnan(sensor.humidity) || isnan(sensor.tempCelsius) || isnan(sensor.tempFarenheit || sensor.tempCelsius > 60 || sensor.humidity > 95 || sensor.tempCelsius < 0)) {} //if invalid measurement occur then it doesn't do anything until next timer.available 


     else
    {
      sendData(sensor);
    }   


    dhtSensorTimer.restart();
 }
 
}



sensorData readSensorData()
{
  sensorData dht11;

     dht11.humidity = dht.readHumidity();
     dht11.tempCelsius = dht.readTemperature();
     dht11.tempFarenheit = dht.readTemperature(true);
     dht11.hindexC = dht.computeHeatIndex(dht11.tempCelsius,dht11.humidity,2);
     dht11.hindexF = dht.computeHeatIndex(dht11.tempFarenheit,dht11.humidity,2);

  return dht11;
}

void sendData(sensorData sens)
{
  #ifdef INSECURE_MODE
    client.setInsecure();
  #endif

   
   
   Serial.print("connecting to ");
   Serial.println(host);
   if (!client.connect(host, httpsPort)) 
   {
      Serial.println("connection failed");
      return;
   }

  if (client.verify(fingerprint, host))
  {
      Serial.println("certificate matches");
  } 
  else 
  {
      Serial.println("certificate doesn't match");
  }

  
  
  String string_tempC = String(sens.tempCelsius, DEC); 
  String string_tempF = String(sens.tempFarenheit, DEC);
  String string_humidity =  String(sens.humidity, DEC);
  String string_hindexC = String(sens.hindexC, DEC);
  String string_hindexF = String(sens.hindexF, DEC);
   
  String url = "/macros/s/" + GAS_ID + "/exec?humidity=" + string_humidity + "&tempCelsius=" + string_tempC + "&tempFahrenheit=" + string_tempF + "&hindexC=" + string_hindexC + "&hindexC=" + string_hindexC + "&hindexF=" + string_hindexF;   


  Serial.print("requesting URL: ");
  Serial.println(url);
  
  
  client.print(String("GET ") + url + " HTTP/1.1\r\n" +     //  get request
         "Host: " + host + "\r\n" +
         "User-Agent: BuildFailureDetectorESP8266\r\n" +
         "Connection: close\r\n\r\n");

 

  Serial.println("request sent");
  while (client.connected()) {
  String line = client.readStringUntil('\n');
  if (line == "\r") {
    Serial.println("headers received");
    break;
  }
  }
  String line = client.readStringUntil('\n');
 
  Serial.println("reply was:");
  Serial.println("==========");
  Serial.println(line);
  Serial.println("==========");
  Serial.println("closing connection");
  
 
}


void writeSensorData(sensorData sens)  //for serial test only
{
      Serial.print(sens.humidity);
      Serial.print("% | ");
      Serial.print(sens.tempCelsius);
      Serial.print("*C | ");
      Serial.print(sens.hindexC);
      Serial.print(" Heat Index (*C) | ");
      Serial.print(sens.hindexF);
      Serial.println(" Heat Index (*F)");
}
