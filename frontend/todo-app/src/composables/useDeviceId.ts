import { v4 as uuidv4 } from 'uuid';

/**
 * Manage the device identifier used to partition todos on the server.  The
 * identifier is persisted in local storage and generated lazily on first
 * access using uuidv4.  Components and composables can import this helper
 * to obtain the current deviceId.
 */
const DEVICE_ID_KEY = 'deviceId';

export function useDeviceId(): string {
  let id = localStorage.getItem(DEVICE_ID_KEY);
  if (!id) {
    id = uuidv4();
    localStorage.setItem(DEVICE_ID_KEY, id);
  }
  return id;
}

export function resetDeviceId(): string {
  const newId = uuidv4();
  localStorage.setItem(DEVICE_ID_KEY, newId);
  return newId;
}