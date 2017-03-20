#include "Coolness.h"

const int8_t Coolness::pins[] = {5, 6, 7, 8, 9, 10, 11, 12};

Coolness::Coolness(const Buggy *buggy) : buggy_(buggy) {
  for (auto &pin : pins) {
    pinMode(pin, OUTPUT);
    digitalWrite(pin, LOW);
  }
}

void Coolness::update() {
  if (buggy_->isGoing()) {
    int16_t progress = ((millis() % loopDuration) / (loopDuration / nPins)) % nPins;
    for (int8_t i = 0; i < nPins; i++) {
      if (progress == i || ((progress + nPins / 2) % nPins) == i) {
        digitalWrite(pins[i], HIGH);
      } else {
        digitalWrite(pins[i], LOW);
      }
    }
  } else {
    for (int8_t i = 0; i < nPins; i++) {
      digitalWrite(pins[i], HIGH);
    }
  }
}
