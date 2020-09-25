/*********
  Rui Santos
  Complete project details at https://randomnerdtutorials.com
*********/

#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
//#include <Fonts/FreeMono9pt7b.h>

#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels

// Declaration for an SSD1306 display connected to I2C (SDA, SCL pins)
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1);

void setup() {
  Serial.begin(9600);

  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) {
    Serial.println("SSD1306 allocation failed");
    for (;;);
  }
  delay(2000);

  //display.setFont(&FreeMono9pt7b);
  display.clearDisplay();
  display.setTextSize(2);       
  display.setTextColor(WHITE);
  display.setCursor(0,0);
  display.display();
  delay(2000);
}
String input;
int c = 0;
void loop() {
  if (Serial.available()) {
    input = Serial.readString();
    if (input != '\n') {
      printDisplay(input);
    }
  }
}

void printDisplay(String inf) {
  display.clearDisplay();
  display.setCursor(0, 0);
  display.print(inf);
  display.println();
  display.display(); // actually display all of the above
}
