# Build stage
FROM node:24-alpine AS build
WORKDIR /app
COPY product-management/package*.json ./
RUN npm ci --legacy-peer-deps
COPY product-management/ .
RUN npm run build -- --configuration=production

# Runtime stage
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/product-management/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
