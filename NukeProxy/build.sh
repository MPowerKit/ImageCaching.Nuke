#!/bin/bash

echo "xcode build"
xcodebuild archive -sdk iphoneos -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphoneos SKIP_INSTALL=NO
xcodebuild archive -sdk iphonesimulator -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -archivePath Output/Output-iphonesimulator SKIP_INSTALL=NO
xcodebuild archive -project NukeProxy.xcodeproj -scheme NukeProxy -configuration Release -destination "generic/platform=macOS,variant=Mac Catalyst"  -archivePath Output/Output-maccatalyst SKIP_INSTALL=NO

echo "create xcframework"
xcodebuild -create-xcframework -framework Output/Output-iphonesimulator.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-iphoneos.xcarchive/Products/Library/Frameworks/NukeProxy.framework -framework Output/Output-maccatalyst.xcarchive/Products/Library/Frameworks/NukeProxy.framework -output Output/NukeProxy.xcframework

echo "sharpie bind"
sharpie bind --sdk=iphoneos17.2 --output="Output" --namespace="MPowerKit.NukeProxy" --scope="Output/NukeProxy.xcframework/ios-arm64/NukeProxy.framework/Headers" "Output/NukeProxy.xcframework/ios-arm64/NukeProxy.framework/Headers/NukeProxy-Swift.h"
