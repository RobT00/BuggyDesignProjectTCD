#pragma once

#include "HashMap.h"
#include "Functions.h"

#include <Arduino.h>

class CommTrans {
 public:
    CommTrans() = delete;
    CommTrans(short ID) : my_ID(ID) {}
    void init() const;
    void writeXbee(String command) const;
    void processCommand(char c);
    void addHandler(String command, VoidFunction handler);
    void setDefaultHandler(StringVoidFunction handler);

 private:
    String message;
    HashMap handlers;
    StringVoidFunction defaultHandler = NULL;
    const short my_ID;
};
