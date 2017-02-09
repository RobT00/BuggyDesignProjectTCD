#include "../Functions.h"
#include "../HashMap.h"

#include <iostream>
#include <cassert>

using namespace std;

int handlerResult = 0;

void stopHandler() {
    handlerResult = 1;
}

void goHandler() {
    handlerResult = 2;
}

void leftHandler() {
    handlerResult = 3;
}

void rightHandler() {
    handlerResult = 4;
}

int main() {
    HashMap map;
    map.add("stop", &stopHandler);
    map.add("go", &goHandler);
    map.add("left", &leftHandler);
    map.add("right", &rightHandler);

    assert(map.size() == 4);

    VoidFunction f;

    f = map.get("stop");
    assert(f != nullptr);
    f();
    assert(handlerResult == 1);

    f = map.get("go");
    assert(f != nullptr);
    f();
    assert(handlerResult == 2);

    f = map.get("left");
    assert(f != nullptr);
    f();
    assert(handlerResult == 3);

    f = map.get("right");
    assert(f != nullptr);
    f();
    assert(handlerResult == 4);

    assert(map.get("abc") == nullptr);

    cout << "Tests passed" << endl;

    return 0;
}
