REPORT="acceptance-tests/Protractor/cucumber_report.xml"
echo "Check if $REPORT exists or not"
if [ ! -f $REPORT ]; then
    echo "$REPORT file not found!"
    exit 1
fi

echo "Check if the file contains testsuite tag or not"
if ! grep -w "<testsuite" $REPORT; then
    echo "No testsuite found"
    exit 1
fi

echo "Check if the file contains testcase tag or not"
if ! grep -w "<testcase" $REPORT; then
    echo "No testcase found"
    exit 1
fi

echo "Check if the file contains error result or not"
if grep -w "<error message=" $REPORT; then
    echo "Found error result"
    exit 1
fi

echo "All tests passed"
exit 0
