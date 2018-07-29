#!/bin/sh

set +u


PROG_DIR=`dirname "$0"` || {
    echo "'dirname' failed" 1>&2
    exit 2
}

CLI=`which cli` || {
    echo "Missing cli command." 1>&2
    echo "Is mono installed?"
    exit 2
}

exec "${CLI}" "${PROG_DIR}/ikvmc.exe" "$@"
