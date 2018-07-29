#/bin/sh!

echo "Defining environment..."

set +u

#change this in correspondense to the release version represented by SVN base-one head revision:
HSQLDB_VERSION=1.8.1.3

## resolve links
PRG="$0"
PROG_NAME=`basename "$0"`

while [ -h "${PRG}" ] ; do
  LS=`ls -ld "${PRG}"`
  LNK=`expr "${LS}" : '.*-> \(.*\)$'`
  if expr "${LNK}" : '/.*' > /dev/null; then
    PRG="${LNK}"
  else
    PRG=`dirname "${PRG}"`"/${LNK}"
  fi
done

PROG_DIR=`dirname "${PRG}"`/
PROG_DIR=`cd "${PROG_DIR}" > /dev/null && pwd`

# configure relative paths...

HSQLDB_SNK=${PROG_DIR}/../Org.Hsqldb.snk
HSQLDB_HOME=${PROG_DIR}/../../../../base-one/trunk
HSQLDB_GLUE_HOME=${PROG_DIR}/Etc/hsqldb-glue
RESTEST_HOME=${PROG_DIR}/Etc/restest/resdb
ANT_HOME=${PROG_DIR}/Etc/ant

# and executables

ANT=${ANT_HOME}/bin/ant
IKVMC=${PROG_DIR}/ikvmc.sh

# absolutize relative paths if possible

[ -f ${HSQLDB_SNK} ] && {
  HSQLDB_SNK=`cd "${PROG_DIR}/.." > /dev/null && pwd`/Org.Hsqldb.snk
}

[ -d ${HSQLDB_HOME} ] && {
  HSQLDB_HOME=`cd "${HSQLDB_HOME}" > /dev/null && pwd`
}

# validate / fixup environment

echo "Validating environment..."

[ -f ${HSQLDB_SNK} ] || {
    echo "Missing code signing key: ${HSQLDB_SNK} " 1>&2
    exit 2
}

[ -d ${HSQLDB_HOME} ] || {
    echo "HSQLDB base-one sources need to be checked out..." 1>&2
    echo "From the command line, try something like:" 1>&2
    echo "mkdir \"${HSQLDB_HOME}\"" 1>&2
    echo "cd \"${HSQLDB_HOME}\"" 1>&2
    echo "svn co https://hsqldb.svn.sourceforge.net/svnroot/hsqldb/base-one/trunk ." 1>&2
    exit 2
}

[ -d ${HSQLDB_GLUE_HOME} ] || {
    echo "Missing HSQLDB_GLUE_HOME directory: ${HSQLDB_GLUE_HOME} " 1>&2
    exit 2
}

[ -d ${RESTEST_HOME} ] || {
    echo "Missing RESTEST_HOME directory: ${RESTEST_HOME} " 1>&2
    exit 2
}

[ -d ${ANT_HOME} ] || {
    echo "Missing ANT_HOME directory: ${ANT_HOME} " 1>&2
    exit 2
}

[ -x ${ANT} ] || {
    chmod +x ${ANT}
    [ -x ${ANT} ] || {
        echo "Missing or non-executable ANT script: ${ANT} " 1>&2
        exit 2
    }
}

CLI=`which cli` || {
    echo "Missing cli command." 1>&2
    echo "Is mono installed?" 1>&2
    exit 2
}

[ -x ${IKVMC} ] || {
    chmod +x ${IKVMC}
    [ -x ${IKVMC} ] || {
        echo "Missing or non-executable ikvmc: ${IKVMC} " 1>&2
        exit 2
    }
}

echo "Environment validated."

# build jars

echo "Starting ant builds..."

${ANT} -f "${HSQLDB_HOME}/build/build.xml" hsqldb || {
    echo "Failed to build hsqldb.jar." 1>&2
    exit 2
}

${ANT} -f "${HSQLDB_GLUE_HOME}/build.xml" jar || {
    echo "Failed to build hsqldb-glue.jar." 1>&2
    exit 2
}

${ANT} -f "${RESTEST_HOME}/build.xml" jar || {
    echo "Failed to build resdb.jar." 1>&2
    exit 2
}

echo "Finished ant builds."

[ -f "${PROG_DIR}/Org.Hsqldb.dll" ] && {
    echo "Backing up old Org.Hsqldb.dll assembly to Org.Hsqldb.dll~"
    mv "${PROG_DIR}/Org.Hsqldb.dll" "${PROG_DIR}/Org.Hsqldb.dll~"
}

# make assemblies

echo "Making Org.Hsqldb.dll library assembly..."

"${CLI}" \
  "${PROG_DIR}/ikvmc.exe" \
  -assembly:Org.Hsqldb \
  "-keyfile:${HSQLDB_SNK}" \
  -target:library \
  "-version:${HSQLDB_VERSION}" \
  "${HSQLDB_HOME}/lib/hsqldb.jar" \
  "${HSQLDB_HOME}/lib/servlet.jar" \
  "${HSQLDB_GLUE_HOME}/dist/hsqldb-glue.jar" 

echo "Making resdb.dll library assembly..."

"${CLI}" \
  "${PROG_DIR}/ikvmc.exe" \
  -assembly:resdb \
  "-keyfile:${HSQLDB_SNK}" \
  -target:library \
  "-version:${HSQLDB_VERSION}" \
  "${RESTEST_HOME}/dist/resdb.jar"

echo "Finished making assemblies."