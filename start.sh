#!/bin/sh

# Keep the browser terminal available even when the current version of the
# chess program exits. Press Enter in the terminal to launch it again.
while true; do
    dotnet "/app/chess 2.0.dll"
    exit_code=$?

    printf '\n\nThe chess application has exited (code: %s).\n' "$exit_code"
    printf 'Press Enter to restart it...'
    IFS= read -r _
done
