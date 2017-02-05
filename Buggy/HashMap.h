#pragma once

#include "Functions.h"

#include <cstdint>

class HashMap {
    private:
        typedef const char* cstring;
        typedef uint32_t hash_t;
        typedef uint16_t size_t;

        hash_t* _keys = nullptr;
        VoidFunction* _handlers = nullptr;
        size_t _size = 0;

        static hash_t hashString(cstring string);

    public:
        HashMap() {}
        ~HashMap();

        /**
         * Adds handler to the map for the given command
         * Will overwrite previous handler if key already exists!
         */
        void add(cstring key, VoidFunction handler);

        /**
         * Returns the handler stored for a given command.
         * Returns nullptr if key not in the map
         */
        VoidFunction get(cstring key) const;

        /**
         * Returns the number of items stored in the map
         */
        size_t size() const { return _size; }
};
