#!/bin/bash

# Tom & Jerry UI Test Runner Script
# This script helps run UI tests with different configurations

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Default values
HEADLESS=true
BROWSER=chromium
SCREENSHOT=false
VERBOSE=false
FILTER=""
TIMEOUT=300

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to show usage
show_usage() {
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -h, --help              Show this help message"
    echo "  -v, --visible           Run tests with visible browser (default: headless)"
    echo "  -b, --browser BROWSER   Set browser (chromium, firefox, webkit) (default: chromium)"
    echo "  -s, --screenshot        Take screenshots on failure"
    echo "  -f, --filter FILTER     Run tests matching filter"
    echo "  -t, --timeout SECONDS   Set test timeout (default: 300)"
    echo "  --verbose               Enable verbose output"
    echo "  --install               Install Playwright browsers"
    echo "  --all                   Run all test categories"
    echo "  --pages                 Run page tests only"
    echo "  --components            Run component tests only"
    echo "  --responsive            Run responsive tests only"
    echo "  --accessibility         Run accessibility tests only"
    echo "  --navigation            Run navigation tests only"
    echo ""
    echo "Examples:"
    echo "  $0 --visible --screenshot"
    echo "  $0 --filter HomePageTests"
    echo "  $0 --browser firefox --pages"
    echo "  $0 --install"
}

# Function to check if application is running
check_application() {
    print_status "Checking if Tom & Jerry application is running..."
    
    if curl -s -f http://localhost:5116 > /dev/null 2>&1; then
        print_success "Application is running on http://localhost:5116"
    else
        print_error "Application is not running on http://localhost:5116"
        print_warning "Please start the application first:"
        print_warning "  cd src && dotnet run"
        exit 1
    fi
}

# Function to install Playwright browsers
install_playwright() {
    print_status "Installing Playwright browsers..."
    dotnet test -- --playwright-install
    print_success "Playwright browsers installed successfully"
}

# Function to run tests
run_tests() {
    local test_args=""
    
    # Build test arguments
    if [ "$HEADLESS" = false ]; then
        test_args="$test_args -- --headless=false"
    fi
    
    if [ "$SCREENSHOT" = true ]; then
        test_args="$test_args -- --screenshot"
    fi
    
    if [ "$VERBOSE" = true ]; then
        test_args="$test_args --logger \"console;verbosity=detailed\""
    fi
    
    if [ -n "$FILTER" ]; then
        test_args="$test_args --filter \"$FILTER\""
    fi
    
    if [ "$BROWSER" != "chromium" ]; then
        test_args="$test_args -- --browser=$BROWSER"
    fi
    
    # Set timeout
    export DOTNET_TEST_TIMEOUT=$TIMEOUT
    
    print_status "Running tests with arguments: $test_args"
    
    # Run the tests
    if dotnet test $test_args; then
        print_success "All tests passed!"
    else
        print_error "Some tests failed!"
        exit 1
    fi
}

# Parse command line arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -h|--help)
            show_usage
            exit 0
            ;;
        -v|--visible)
            HEADLESS=false
            shift
            ;;
        -b|--browser)
            BROWSER="$2"
            shift 2
            ;;
        -s|--screenshot)
            SCREENSHOT=true
            shift
            ;;
        -f|--filter)
            FILTER="$2"
            shift 2
            ;;
        -t|--timeout)
            TIMEOUT="$2"
            shift 2
            ;;
        --verbose)
            VERBOSE=true
            shift
            ;;
        --install)
            install_playwright
            exit 0
            ;;
        --all)
            FILTER=""
            shift
            ;;
        --pages)
            FILTER="HomePageTests|QuizPageTests|SearchPageTests|StickersPageTests|PlayMediaPageTests"
            shift
            ;;
        --components)
            FILTER="ComponentTests"
            shift
            ;;
        --responsive)
            FILTER="ResponsiveTests"
            shift
            ;;
        --accessibility)
            FILTER="AccessibilityTests"
            shift
            ;;
        --navigation)
            FILTER="NavigationTests"
            shift
            ;;
        *)
            print_error "Unknown option: $1"
            show_usage
            exit 1
            ;;
    esac
done

# Main execution
print_status "Starting Tom & Jerry UI Test Runner"
print_status "Configuration:"
print_status "  Headless: $HEADLESS"
print_status "  Browser: $BROWSER"
print_status "  Screenshot: $SCREENSHOT"
print_status "  Verbose: $VERBOSE"
print_status "  Filter: ${FILTER:-'All tests'}"
print_status "  Timeout: ${TIMEOUT}s"

# Check if application is running
check_application

# Run tests
run_tests

print_success "Test execution completed!"
