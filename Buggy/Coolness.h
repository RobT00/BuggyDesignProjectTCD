#include <Arduino.h>

#include "Buggy.h"

class Coolness {
 public:
    Coolness(const Buggy *);
    void update();

 private:
    const Buggy* buggy_;
    static const int16_t loopDuration = 500;
    static const int8_t nPins = 8;
    static const int8_t pins[nPins];
};
