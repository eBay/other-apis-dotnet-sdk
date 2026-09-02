#!/usr/bin/env bash

# Copyright 2026 eBay Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

set -euo pipefail

echo "Restoring tools..."
dotnet tool restore

echo "Regenerating REST clients via Kiota..."

# EDIS Shipping API
dotnet kiota generate \
  -l csharp \
  -d specs/sell_edelivery_international_shipping_oas3.yaml \
  -c ShippingApiClient \
  -n EBay.OtherApis.Generated.Shipping \
  -o EBay.OtherApis/Generated/Shipping \
  --ebc --co

# Identity API
dotnet kiota generate \
  -l csharp \
  -d specs/commerce_identity_v1_oas3.yaml \
  -c IdentityApiClient \
  -n EBay.OtherApis.Generated.Identity \
  -o EBay.OtherApis/Generated/Identity \
  --ebc --co

# Translation API
dotnet kiota generate \
  -l csharp \
  -d specs/commerce_translation_v1_beta_oas3.yaml \
  -c TranslationApiClient \
  -n EBay.OtherApis.Generated.Translation \
  -o EBay.OtherApis/Generated/Translation \
  --ebc --co

# Vero API
dotnet kiota generate \
  -l csharp \
  -d specs/commerce_vero_v1_oas3.yaml \
  -c VeroApiClient \
  -n EBay.OtherApis.Generated.Vero \
  -o EBay.OtherApis/Generated/Vero \
  --ebc --co

echo "Adding license headers to generated files..."
./add-license-headers.sh ./EBay.OtherApis/Generated

echo "Building SDK..."
DOTNET_ROLL_FORWARD=LatestMajor dotnet build EBay.OtherApis/EBay.OtherApis.csproj

echo "Done."
