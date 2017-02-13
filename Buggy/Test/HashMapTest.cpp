#include "../Functions.h"
#include "../HashMap.h"

#include <iostream>
#include <cassert>
#include <vector>
#include <string>

using namespace std;

class Integer {
    private:
        mutable int i;

    public:
        Integer(int initial) : i(initial) {}
        Integer() : Integer(0) {};
        int get() const { return i; }
        void increment() const { i++; }
};

static const Integer handlerResult;

int main() {
    const vector<const char*> commands = {
        "PING",
        "PONG",
        "LED",
        "CLOCK",
        "ACLOCK",
        "GO",
        "STOP",
        "STRAIGHT",
        "PARK"
    };

    HashMap map;
    for (unsigned int i = 0; i < commands.size(); i++) {
        map.add(commands[i], [] { handlerResult.increment(); });
    }

    assert(map.size() == commands.size());

    for (unsigned int i = 0; i < commands.size(); i++) {
        VoidFunction f = map.get(commands[i]);
        assert(f != nullptr);
        f();
        assert(handlerResult.get() == i + 1);
    }

    cout << "Tests passed" << endl;

    return 0;
}
