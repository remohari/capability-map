FROM node:22-alpine AS build
WORKDIR /app

COPY package.json ./
COPY angular.json ./
COPY tsconfig.json ./
COPY tsconfig.app.json ./
COPY public ./public
COPY src ./src

RUN npm install
RUN npm run build

FROM nginx:1.29-alpine AS runtime
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/capability-map-frontend/browser /usr/share/nginx/html

EXPOSE 8080
